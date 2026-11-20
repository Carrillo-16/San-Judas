using BackendSanJudas.Servicios;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[ApiController,Route("Api/Plantillas")]
public class PlantillasControlador(PlantillaAlumnoPdf plantilla):ControllerBase
{
    [HttpGet("Alumno/Pdf")]
    public IActionResult Descargar() => File(plantilla.Generar(new Dictionary<string,string>(),null,null),"application/pdf","Plantilla_Alumno.pdf");
}
