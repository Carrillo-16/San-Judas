
namespace BackendSanJudas.Modelos.Inscripciones;

public class ConsultarInscripcionesDto
{
    public int IdInscripcion { get; set; }
    public int IdAlumno { get; set; }
    public int IdGrupo { get; set; }
    public int IdCicloEscolar { get; set; }
    public DateOnly FechaInscripcion { get; set; }
    public int IdEncargado { get; set; }
    public string TelefonoEncargado { get; set; } = string.Empty;
}
