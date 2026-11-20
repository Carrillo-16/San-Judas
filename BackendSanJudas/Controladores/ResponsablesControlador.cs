using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Responsables;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/Responsables")]
public class ResponsablesControlador(IResponsablesServicio servicio) : CrudControlador<CrearResponsablesDto,ActualizarResponsablesDto,ConsultarResponsablesDto>(servicio);
