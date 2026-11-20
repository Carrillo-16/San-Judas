using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Calificaciones;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/Calificaciones")]
public class CalificacionesControlador(ICalificacionesServicio servicio) : CrudControlador<CrearCalificacionesDto,ActualizarCalificacionesDto,ConsultarCalificacionesDto>(servicio);
