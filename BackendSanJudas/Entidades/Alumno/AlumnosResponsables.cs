using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSanJudas.Entidades;

public class AlumnosResponsables
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdAlumnoResponsable { get; set; } // Generado automáticamente por SQL Server.
   
    [ForeignKey("Alumnos")]
    public int IdAlumno { get; set; }

    [ForeignKey("Responsables")]
    public int IdResponsable { get; set; }
    
    public int EstadoParentesco { get; set; }
    public Alumnos Alumnos { get; set; } = null!;
    public Responsables Responsables { get; set; } = null!;
}
