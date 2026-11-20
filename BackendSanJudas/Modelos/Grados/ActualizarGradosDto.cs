using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.Grados;

public class ActualizarGradosDto
{
    [Required, StringLength(80)]
    public string Nombre { get; set; } = string.Empty;
    [Range(1,25)]
    public int? EdadReferencia { get; set; }
}
