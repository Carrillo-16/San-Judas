using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Areas;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/Areas")]
public class AreasControlador(IAreasServicio servicio) : CrudControlador<CrearAreasDto,ActualizarAreasDto,ConsultarAreasDto>(servicio);
