using BackendSanJudas.Entidades;


using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Documentos;
using BackendSanJudas.Modelos.Comunes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class DocumentosServicio(DGDbContext contexto,IAlmacenamientoArchivos almacenamiento) : IDocumentosServicio
{
    private static readonly Expression<Func<Documentos,ConsultarDocumentoDto>> Proyeccion = x=>new ConsultarDocumentoDto { IdDocumento=x.IdDocumento, Titulo=x.Titulo, EstadoTipoDocumento=x.EstadoTipoDocumento, IdAlumno=x.IdAlumno, IdResponsable=x.IdResponsable, IdCategoriaDocumento=x.IdCategoriaDocumento, FechaDocumento=x.FechaDocumento, FechaCargaUtc=x.FechaCargaUtc, EsPlantilla=x.EsPlantilla, NombreOriginal=x.NombreOriginal, TamanoBytes=x.TamanoBytes, UrlDescarga="/Api/Documentos/"+x.IdDocumento+"/Archivo" };
    public async Task<PaginaDto<ConsultarDocumentoDto>> ListarAsync(PaginacionDto pagina,int? idAlumno,int? idCategoria,DateOnly? desde,DateOnly? hasta,int? tipo,bool? esPlantilla,CancellationToken cancelacion)
    {
        if (desde>hasta) throw new BadHttpRequestException("El rango de fechas no es válido.");
        var consulta=contexto.Documentos.AsNoTracking();
        if (idAlumno.HasValue) consulta=consulta.Where(x=>x.IdAlumno==idAlumno);
        if (idCategoria.HasValue) consulta=consulta.Where(x=>x.IdCategoriaDocumento==idCategoria);
        if (desde.HasValue) consulta=consulta.Where(x=>x.FechaDocumento>=desde);
        if (hasta.HasValue) consulta=consulta.Where(x=>x.FechaDocumento<=hasta);
        if (tipo.HasValue) consulta=consulta.Where(x=>x.EstadoTipoDocumento==tipo);
        if (esPlantilla.HasValue) consulta=consulta.Where(x=>x.EsPlantilla==esPlantilla);
        return new() { Pagina=pagina.Pagina, TamanoPagina=pagina.TamanoPagina, Total=await consulta.CountAsync(cancelacion), Elementos=await consulta.OrderByDescending(x=>x.FechaDocumento).ThenByDescending(x=>x.IdDocumento).Skip((pagina.Pagina-1)*pagina.TamanoPagina).Take(pagina.TamanoPagina).Select(Proyeccion).ToListAsync(cancelacion) };
    }
    private async Task ValidarAsync(Documentos documento,CancellationToken cancelacion)
    {
        if ((documento.EstadoTipoDocumento<1 || documento.EstadoTipoDocumento>5) || documento.FechaDocumento==default) throw new BadHttpRequestException("Tipo o fecha de documento no válido.");
        if (documento.EstadoTipoDocumento==5)
        {
            if (documento.IdAlumno!=null || documento.IdResponsable!=null || documento.IdCategoriaDocumento==null) throw new BadHttpRequestException("El documento administrativo requiere categoría y no se vincula a un alumno.");
            if (!await contexto.CategoriasDocumentos.AnyAsync(x=>x.IdCategoriaDocumento==documento.IdCategoriaDocumento,cancelacion)) throw new BadHttpRequestException("Categoría no encontrada.",404);
        }
        else
        {
            if (documento.IdCategoriaDocumento!=null || documento.EsPlantilla || documento.IdAlumno==null) throw new BadHttpRequestException("El expediente requiere alumno, sin categoría administrativa ni indicador de plantilla.");
            if (!await contexto.Alumnos.AnyAsync(x=>x.IdAlumno==documento.IdAlumno,cancelacion)) throw new BadHttpRequestException("Alumno no encontrado.",404);
            if (documento.EstadoTipoDocumento==2)
            {
                if (documento.IdResponsable==null || !await contexto.AlumnosResponsables.AnyAsync(x=>x.IdAlumno==documento.IdAlumno && x.IdResponsable==documento.IdResponsable,cancelacion)) throw new BadHttpRequestException("El DPI debe pertenecer a un responsable vinculado al alumno.");
            }
            else if (documento.IdResponsable!=null) throw new BadHttpRequestException("Solo el DPI se asocia a un responsable.");
        }
    }
    public async Task<ConsultarDocumentoDto> SubirAsync(SubirDocumentoDto modelo,CancellationToken cancelacion)
    {
        if (modelo.EstadoTipoDocumento==4 && !string.Equals(Path.GetExtension(modelo.Archivo.FileName),".pdf",StringComparison.OrdinalIgnoreCase)) throw new BadHttpRequestException("La evidencia de adecuación curricular debe ser un PDF.");
        var documento=new Documentos { Titulo=modelo.Titulo.Trim(), EstadoTipoDocumento=modelo.EstadoTipoDocumento, IdAlumno=modelo.IdAlumno, IdResponsable=modelo.IdResponsable, IdCategoriaDocumento=modelo.IdCategoriaDocumento, FechaDocumento=modelo.FechaDocumento, EsPlantilla=modelo.EsPlantilla, FechaCargaUtc=DateTime.UtcNow };
        await ValidarAsync(documento,cancelacion);
        if (modelo.EstadoTipoDocumento==1 && await contexto.Documentos.AnyAsync(x=>x.IdAlumno==modelo.IdAlumno && x.EstadoTipoDocumento==1,cancelacion)) throw new BadHttpRequestException("El alumno ya tiene foto. Elimine la anterior antes de subir otra.",409);
        var guardado=await almacenamiento.GuardarAsync(modelo.Archivo,modelo.EstadoTipoDocumento==1,cancelacion);
        documento.RutaRelativa=guardado.Ruta; documento.TipoContenido=guardado.TipoContenido;
        documento.NombreOriginal=Path.GetFileName(modelo.Archivo.FileName.Replace('\\','/'));
        if (documento.NombreOriginal.Length>255) documento.NombreOriginal=documento.NombreOriginal[^255..];
        documento.TamanoBytes=modelo.Archivo.Length;
        try { contexto.Add(documento); await ValidacionesRegistro.GuardarAsync(contexto,cancelacion); await contexto.SaveChangesAsync(cancelacion); }
        catch { almacenamiento.Eliminar(guardado.Ruta); throw; }
        return await contexto.Documentos.Where(x=>x.IdDocumento==documento.IdDocumento).Select(Proyeccion).SingleAsync(cancelacion);
    }
    public async Task ActualizarAsync(int id,ActualizarDocumentoDto modelo,CancellationToken cancelacion)
    {
        var documento=await contexto.Documentos.FindAsync([id],cancelacion) ?? throw new BadHttpRequestException("Documento no encontrado.",404);
        documento.Titulo=modelo.Titulo.Trim(); documento.FechaDocumento=modelo.FechaDocumento; documento.IdCategoriaDocumento=modelo.IdCategoriaDocumento; documento.EsPlantilla=modelo.EsPlantilla;
        await ValidarAsync(documento,cancelacion); await ValidacionesRegistro.GuardarAsync(contexto,cancelacion); await contexto.SaveChangesAsync(cancelacion);
    }
    public async Task<(Stream Archivo,string TipoContenido,string Nombre)> DescargarAsync(int id,CancellationToken cancelacion)
    {
        var documento=await contexto.Documentos.AsNoTracking().SingleOrDefaultAsync(x=>x.IdDocumento==id,cancelacion) ?? throw new BadHttpRequestException("Documento no encontrado.",404);
        return (almacenamiento.Abrir(documento.RutaRelativa),documento.TipoContenido,documento.NombreOriginal);
    }
    public async Task EliminarAsync(int id,CancellationToken cancelacion)
    {
        var documento=await contexto.Documentos.FindAsync([id],cancelacion) ?? throw new BadHttpRequestException("Documento no encontrado.",404);
        // Guardar primero el cambio en SQL; después retirar el archivo.
        contexto.Remove(documento); await ValidacionesRegistro.GuardarAsync(contexto,cancelacion); await contexto.SaveChangesAsync(cancelacion);
        almacenamiento.Eliminar(documento.RutaRelativa);
    }
}
