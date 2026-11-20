using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Alumnos;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/Alumnos")]
public class AlumnosControlador(IAlumnosServicio servicio) : CrudControlador<CrearAlumnosDto,ActualizarAlumnosDto,ConsultarAlumnosDto>(servicio);
