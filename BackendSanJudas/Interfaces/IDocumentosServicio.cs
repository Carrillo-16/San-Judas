using BackendSanJudas.Modelos.Documentos;
using BackendSanJudas.Modelos.Comunes;

namespace BackendSanJudas.Interfaces;
public interface IDocumentosServicio
{
    Task<PaginaDto<ConsultarDocumentoDto>> ListarAsync(PaginacionDto pagina,int? idAlumno,int? idCategoria,DateOnly? desde,DateOnly? hasta,int? tipo,bool? esPlantilla,CancellationToken cancelacion);
    Task<ConsultarDocumentoDto> SubirAsync(SubirDocumentoDto modelo,CancellationToken cancelacion);
    Task ActualizarAsync(int id,ActualizarDocumentoDto modelo,CancellationToken cancelacion);
    Task<(Stream Archivo,string TipoContenido,string Nombre)> DescargarAsync(int id,CancellationToken cancelacion);
    Task EliminarAsync(int id,CancellationToken cancelacion);
}
