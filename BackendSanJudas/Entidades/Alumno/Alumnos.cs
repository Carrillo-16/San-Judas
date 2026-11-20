using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackendSanJudas.Entidades;

public class Alumnos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdAlumno { get; set; } // Generado automáticamente por SQL Server.

    public string NombresAlumno { get; set; } = string.Empty;

    public string ApellidosAlumno { get; set; } = string.Empty;

    public string CodigoAlumno { get; set; } = string.Empty;

    public DateOnly FechaNacimiento { get; set; }

    public string? Genero { get; set; } // Masculino o Femenino; opcional si no se registró.

    public bool TieneAdecuacionCurricular { get; set; }
}
