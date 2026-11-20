using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Grupos;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/Grupos")]
public class GruposControlador(IGruposServicio servicio) : CrudControlador<CrearGruposDto,ActualizarGruposDto,ConsultarGruposDto>(servicio);
