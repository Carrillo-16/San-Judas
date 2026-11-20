using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.GradosAreas;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/GradosAreas")]
public class GradosAreasControlador(IGradosAreasServicio servicio) : CrudControlador<CrearGradosAreasDto,ActualizarGradosAreasDto,ConsultarGradosAreasDto>(servicio);
