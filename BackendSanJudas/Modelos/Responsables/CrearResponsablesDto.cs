using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.Responsables;

public class CrearResponsablesDto
{
    [Required, StringLength(200)]
    public string NombreCompleto { get; set; } = string.Empty;

    public DateOnly? FechaNacimiento { get; set; }
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El DPI debe contener 13 dígitos.")]
    public string? Dpi { get; set; }
    [Required, RegularExpression(@"^[0-9+()\s-]{8,25}$")]
    public string Telefono { get; set; } = string.Empty;
}
