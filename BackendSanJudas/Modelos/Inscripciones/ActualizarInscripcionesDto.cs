using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.Inscripciones;

public class ActualizarInscripcionesDto
{
    [Range(1,int.MaxValue)] public int IdAlumno { get; set; }
    [Range(1,int.MaxValue)] public int IdGrupo { get; set; }
    public DateOnly FechaInscripcion { get; set; }
    [Range(1,int.MaxValue)] public int IdEncargado { get; set; }
    [Required, RegularExpression(@"^[0-9+()\s-]{8,25}$")] public string TelefonoEncargado { get; set; } = string.Empty;
}
