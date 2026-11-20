using BackendSanJudas.Entidades;

using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Comunes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public abstract class CrudServicio<TEntidad,TCrear,TActualizar,TConsultar>(DGDbContext contexto) : ICrudServicio<TCrear,TActualizar,TConsultar> where TEntidad : class, new()
{
    protected DGDbContext Contexto { get; } = contexto;
    protected abstract Expression<Func<TEntidad,TConsultar>> Proyeccion { get; }
    protected abstract string NombreId { get; }
    protected abstract void AplicarCrear(TEntidad entidad, TCrear modelo);
    protected abstract void AplicarActualizar(TEntidad entidad, TActualizar modelo);
    protected virtual Task ValidarAsync(TEntidad entidad, bool nuevo, CancellationToken cancelacion) => Task.CompletedTask;
    protected virtual Task ValidarEliminarAsync(TEntidad entidad, CancellationToken cancelacion) => Task.CompletedTask;
    protected async Task<TEntidad> BuscarAsync(int id, CancellationToken cancelacion) => await Contexto.Set<TEntidad>().FindAsync([id], cancelacion) ?? throw new BadHttpRequestException("Registro no encontrado.",404);
    public async Task<PaginaDto<TConsultar>> ListarAsync(PaginacionDto paginacion, CancellationToken cancelacion)
    {
        var consulta=Contexto.Set<TEntidad>().AsNoTracking();
        return new() { Pagina=paginacion.Pagina, TamanoPagina=paginacion.TamanoPagina, Total=await consulta.CountAsync(cancelacion), Elementos=await consulta.OrderBy(x=>EF.Property<int>(x,NombreId)).Skip((paginacion.Pagina-1)*paginacion.TamanoPagina).Take(paginacion.TamanoPagina).Select(Proyeccion).ToListAsync(cancelacion) };
    }
    public async Task<TConsultar> ObtenerAsync(int id, CancellationToken cancelacion) => await Contexto.Set<TEntidad>().AsNoTracking().Where(x=>EF.Property<int>(x,NombreId)==id).Select(Proyeccion).SingleOrDefaultAsync(cancelacion) ?? throw new BadHttpRequestException("Registro no encontrado.",404);
    public async Task<TConsultar> CrearAsync(TCrear modelo, CancellationToken cancelacion)
    {
        var entidad=new TEntidad(); AplicarCrear(entidad,modelo); await ValidarAsync(entidad,true,cancelacion);
        Contexto.Add(entidad); await ValidacionesRegistro.GuardarAsync(Contexto,cancelacion); await Contexto.SaveChangesAsync(cancelacion);
        int id=(int)Contexto.Entry(entidad).Property(NombreId).CurrentValue!;
        return await ObtenerAsync(id,cancelacion);
    }
    public async Task ActualizarAsync(int id,TActualizar modelo,CancellationToken cancelacion)
    {
        var entidad=await BuscarAsync(id,cancelacion); AplicarActualizar(entidad,modelo); await ValidarAsync(entidad,false,cancelacion); await ValidacionesRegistro.GuardarAsync(Contexto,cancelacion); await Contexto.SaveChangesAsync(cancelacion);
    }
    public async Task EliminarAsync(int id,CancellationToken cancelacion)
    {
        var entidad=await BuscarAsync(id,cancelacion); await ValidarEliminarAsync(entidad,cancelacion); await ValidacionesRegistro.EliminarAsync(Contexto,entidad,cancelacion); Contexto.Remove(entidad); await ValidacionesRegistro.GuardarAsync(Contexto,cancelacion); await Contexto.SaveChangesAsync(cancelacion);
    }
}
