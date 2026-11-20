using BackendSanJudas.Interfaces;

using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[ApiController, Route("Api/Boletas")]
public class BoletasControlador(IBoletasServicio servicio) : ControllerBase
{
    [HttpGet("Inscripciones/{idInscripcion:int}")] public async Task<IActionResult> Obtener(int idInscripcion,CancellationToken cancelacion) => Ok(await servicio.ObtenerAsync(idInscripcion,cancelacion));
    [HttpGet("Grupos/{idGrupo:int}/Consolidado")] public async Task<IActionResult> Consolidado(int idGrupo,CancellationToken cancelacion) => Ok(await servicio.ConsolidadoAsync(idGrupo,cancelacion));
}
