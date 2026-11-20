using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Grados;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/Grados")]
public class GradosControlador(IGradosServicio servicio) : CrudControlador<CrearGradosDto,ActualizarGradosDto,ConsultarGradosDto>(servicio);
