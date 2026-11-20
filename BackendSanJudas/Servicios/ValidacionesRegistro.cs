using BackendSanJudas.Entidades;
using Microsoft.EntityFrameworkCore;
namespace BackendSanJudas.Servicios;
public static class ValidacionesRegistro
{
    public static async Task GuardarAsync(DGDbContext contexto, CancellationToken cancelacion)
    {
        foreach (var entrada in contexto.ChangeTracker.Entries().Where(e=>e.State is EntityState.Added or EntityState.Modified).ToList())
        {
            foreach (var fk in entrada.Metadata.GetForeignKeys())
            {
                // Las FK compuestas se verifican mediante las reglas de inscripción.
                if (fk.Properties.Count!=1 || !fk.PrincipalKey.IsPrimaryKey()) continue;
                object? id=entrada.Property(fk.Properties[0].Name).CurrentValue;
                if (id!=null && await contexto.FindAsync(fk.PrincipalEntityType.ClrType,[id],cancelacion)==null)
                    throw new BadHttpRequestException("El registro relacionado no existe.",400);
            }
            var pk=entrada.Metadata.FindPrimaryKey()!.Properties.Single().Name;
            int propio=(int)(entrada.Property(pk).CurrentValue ?? 0);
            foreach (var indice in entrada.Metadata.GetIndexes().Where(i=>i.IsUnique))
            {
                if (entrada.Entity is Documentos d && indice.Properties.Any(p=>p.Name=="EstadoTipoDocumento") && d.EstadoTipoDocumento!=1) continue;
                if (entrada.Entity is AlumnosResponsables ar && indice.Properties.Any(p=>p.Name=="EstadoParentesco") && ar.EstadoParentesco==3) continue;
                var campos=indice.Properties.Select(p=>p.Name).ToArray();
                var valores=campos.Select(n=>entrada.Property(n).CurrentValue).ToArray();
                if (valores.Any(v=>v==null)) continue;
                if (await ExisteAsync(contexto,entrada.Metadata.ClrType,campos,valores,pk,propio,cancelacion))
                    throw new BadHttpRequestException("Ya existe un registro con estos datos ("+string.Join(", ",campos)+").",409);
            }
        }
    }
    private static Task<bool> ExisteAsync(DGDbContext contexto,Type tipo,string[] campos,object?[] valores,string pk,int propio,CancellationToken ct)
    {
        var metodo=typeof(ValidacionesRegistro).GetMethod(nameof(ConsultarAsync),System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Static)!.MakeGenericMethod(tipo);
        return (Task<bool>)metodo.Invoke(null,[contexto,campos,valores,pk,propio,ct])!;
    }
    private static Task<bool> ConsultarAsync<T>(DGDbContext contexto,string[] campos,object?[] valores,string pk,int propio,CancellationToken ct) where T:class
    {
        var parametro=System.Linq.Expressions.Expression.Parameter(typeof(T),"x");
        System.Linq.Expressions.Expression condicion=System.Linq.Expressions.Expression.NotEqual(System.Linq.Expressions.Expression.Property(parametro,pk),System.Linq.Expressions.Expression.Constant(propio));
        for(int i=0;i<campos.Length;i++)
        {
            var propiedad=System.Linq.Expressions.Expression.Property(parametro,campos[i]);
            var valor=System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Constant(valores[i]),propiedad.Type);
            condicion=System.Linq.Expressions.Expression.AndAlso(condicion,System.Linq.Expressions.Expression.Equal(propiedad,valor));
        }
        return contexto.Set<T>().AnyAsync(System.Linq.Expressions.Expression.Lambda<Func<T,bool>>(condicion,parametro),ct);
    }
    public static async Task EliminarAsync(DGDbContext contexto,object entidad,CancellationToken ct)
    {
        var entrada=contexto.Entry(entidad);
        var pk=entrada.Metadata.FindPrimaryKey()!.Properties.Single().Name;
        int id=(int)entrada.Property(pk).CurrentValue!;
        foreach(var fk in entrada.Metadata.GetReferencingForeignKeys().Where(f=>f.Properties.Count==1))
        {
            string clave=fk.DeclaringEntityType.FindPrimaryKey()!.Properties.Single().Name;
            if(await ExisteAsync(contexto,fk.DeclaringEntityType.ClrType,[fk.Properties[0].Name],[id],clave,0,ct))
                throw new BadHttpRequestException("No se puede eliminar: tiene registros relacionados.",409);
        }
    }
}
