using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSanJudas.Entidades;

public class Inscripciones
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdInscripcion { get; set; } // Generado automáticamente por SQL Server.
    [ForeignKey("Alumnos")]
    public int IdAlumno { get; set; }
    [ForeignKey("Grupos")]
    public int IdGrupo { get; set; }
    [ForeignKey("CiclosEscolares")]
    public int IdCicloEscolar { get; set; }
    public DateOnly FechaInscripcion { get; set; }

    [ForeignKey("Responsables")]
    public int IdEncargado { get; set; }
    public string TelefonoEncargado { get; set; } = string.Empty;
    public Alumnos Alumnos { get; set; } = null!;
    public Grupos Grupos { get; set; } = null!;
    public CiclosEscolares CiclosEscolares { get; set; } = null!;
    public Responsables Responsables { get; set; } = null!;
}
