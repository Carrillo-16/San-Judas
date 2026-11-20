using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackendSanJudas.Entidades;

public class Responsables
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdResponsable { get; set; } // Generado automáticamente por SQL Server.

    public string NombreCompleto { get; set; } = string.Empty;

    public DateOnly? FechaNacimiento { get; set; }

    public string? Dpi { get; set; }

    public string Telefono { get; set; } = string.Empty;
}
