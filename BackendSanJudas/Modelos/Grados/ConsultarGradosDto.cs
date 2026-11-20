using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.Grados;

public class ConsultarGradosDto
{
    public int IdGrado { get; set; }
    [Required, StringLength(80)]
    public string Nombre { get; set; } = string.Empty;
    [Range(1,25)]
    public int? EdadReferencia { get; set; }
}
