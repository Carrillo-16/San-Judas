using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.AlumnosResponsables;

using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[ApiController, Route("Api/Alumnos/{idAlumno:int}/Responsables")]
public class AlumnosResponsablesControlador(IAlumnosResponsablesServicio servicio) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> Listar(int idAlumno,CancellationToken cancelacion) => Ok(await servicio.ListarAsync(idAlumno,cancelacion));
    [HttpPost] public async Task<IActionResult> Vincular(int idAlumno,VincularResponsableDto modelo,CancellationToken cancelacion) => StatusCode(201,await servicio.VincularAsync(idAlumno,modelo,cancelacion));
    [HttpDelete("{idResponsable:int}")] public async Task<IActionResult> Desvincular(int idAlumno,int idResponsable,CancellationToken cancelacion) { await servicio.DesvincularAsync(idAlumno,idResponsable,cancelacion); return NoContent(); }
}
