
namespace BackendSanJudas.Modelos.AlumnosResponsables;
public class ConsultarAlumnoResponsableDto
{
    public int IdAlumnoResponsable { get; set; }
    public int IdAlumno { get; set; }
    public int IdResponsable { get; set; }
    public int EstadoParentesco { get; set; }
    public string NombreCompleto { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string? Dpi { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
}
