using BackendSanJudas.Interfaces;

using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[ApiController, Route("Api/Inscripciones")]
public class InscripcionPdfControlador(IInscripcionPdfServicio servicio) : ControllerBase
{
    [HttpGet("{idInscripcion:int}/Pdf")] public async Task<IActionResult> Descargar(int idInscripcion,CancellationToken cancelacion)
    {
        Response.Headers.CacheControl="no-store";
        return File(await servicio.GenerarAsync(idInscripcion,cancelacion),"application/pdf",$"Inscripcion-{idInscripcion}.pdf");
    }
}
