using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.Areas;

public class ConsultarAreasDto
{
    public int IdArea { get; set; }
    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
}
