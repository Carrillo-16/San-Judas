using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.Areas;

public class CrearAreasDto
{
    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
}
