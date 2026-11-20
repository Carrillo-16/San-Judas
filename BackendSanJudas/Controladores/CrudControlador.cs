using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Comunes;

using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[ApiController]
public abstract class CrudControlador<TCrear,TActualizar,TConsultar>(ICrudServicio<TCrear,TActualizar,TConsultar> servicio) : ControllerBase
{
    [HttpGet] public Task<PaginaDto<TConsultar>> Listar([FromQuery] PaginacionDto paginacion,CancellationToken cancelacion) => servicio.ListarAsync(paginacion,cancelacion);
    [HttpGet("{id:int}")] public Task<TConsultar> Obtener(int id,CancellationToken cancelacion) => servicio.ObtenerAsync(id,cancelacion);
    [HttpPost] public async Task<ActionResult<TConsultar>> Crear(TCrear modelo,CancellationToken cancelacion)
    {
        var resultado=await servicio.CrearAsync(modelo,cancelacion); return StatusCode(201,resultado);
    }
    [HttpPut("{id:int}")] public async Task<IActionResult> Actualizar(int id,TActualizar modelo,CancellationToken cancelacion) { await servicio.ActualizarAsync(id,modelo,cancelacion); return NoContent(); }
    [HttpDelete("{id:int}")] public async Task<IActionResult> Eliminar(int id,CancellationToken cancelacion) { await servicio.EliminarAsync(id,cancelacion); return NoContent(); }
}
