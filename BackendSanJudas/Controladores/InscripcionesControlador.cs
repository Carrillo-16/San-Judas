using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Inscripciones;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/Inscripciones")]
public class InscripcionesControlador(IInscripcionesServicio servicio) : CrudControlador<CrearInscripcionesDto,ActualizarInscripcionesDto,ConsultarInscripcionesDto>(servicio);
