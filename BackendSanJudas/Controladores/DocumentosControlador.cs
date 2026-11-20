using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Documentos;
using BackendSanJudas.Modelos.Comunes;


using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[ApiController, Route("Api/Documentos")]
public class DocumentosControlador(IDocumentosServicio servicio) : ControllerBase
{
    [HttpGet] public Task<PaginaDto<ConsultarDocumentoDto>> Listar([FromQuery] PaginacionDto pagina,[FromQuery] int? idAlumno,[FromQuery] int? idCategoria,[FromQuery] DateOnly? desde,[FromQuery] DateOnly? hasta,[FromQuery] int? tipo,[FromQuery] bool? esPlantilla,CancellationToken cancelacion) => servicio.ListarAsync(pagina,idAlumno,idCategoria,desde,hasta,tipo,esPlantilla,cancelacion);
    [HttpPost, Consumes("multipart/form-data"), RequestSizeLimit(11534336)] public async Task<IActionResult> Subir([FromForm] SubirDocumentoDto modelo,CancellationToken cancelacion) => StatusCode(201,await servicio.SubirAsync(modelo,cancelacion));
    [HttpPut("{id:int}")] public async Task<IActionResult> Actualizar(int id,ActualizarDocumentoDto modelo,CancellationToken cancelacion) { await servicio.ActualizarAsync(id,modelo,cancelacion); return NoContent(); }
    [HttpGet("{id:int}/Archivo")] public async Task<IActionResult> Descargar(int id,CancellationToken cancelacion)
    {
        var archivo=await servicio.DescargarAsync(id,cancelacion); Response.Headers.CacheControl="no-store"; Response.Headers.XContentTypeOptions="nosniff";
        if (archivo.TipoContenido.StartsWith("image/",StringComparison.Ordinal)) return File(archivo.Archivo,archivo.TipoContenido,enableRangeProcessing:true);
        return File(archivo.Archivo,archivo.TipoContenido,archivo.Nombre,enableRangeProcessing:true);
    }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Eliminar(int id,CancellationToken cancelacion) { await servicio.EliminarAsync(id,cancelacion); return NoContent(); }
}
