using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.CiclosEscolares;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/CiclosEscolares")]
public class CiclosEscolaresControlador(ICiclosEscolaresServicio servicio) : CrudControlador<CrearCiclosEscolaresDto,ActualizarCiclosEscolaresDto,ConsultarCiclosEscolaresDto>(servicio);
