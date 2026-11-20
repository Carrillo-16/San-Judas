using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.Alumnos;

public class ActualizarAlumnosDto
{
    [Required, StringLength(100)]
    public string NombresAlumno { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string ApellidosAlumno { get; set; } = string.Empty;
    [Required, StringLength(30)]
    public string CodigoAlumno { get; set; } = string.Empty;
    [Required]
    public DateOnly FechaNacimiento { get; set; }

    public string? Genero { get; set; } // Masculino o Femenino; opcional si no se registró.

    public bool TieneAdecuacionCurricular { get; set; }
}
