using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSanJudas.Entidades;

public class Calificaciones
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCalificacion { get; set; } // Generado automáticamente por SQL Server.
    [ForeignKey("Inscripciones")]
    public int IdInscripcion { get; set; }

    [ForeignKey("Areas")]
    public int IdArea { get; set; }
    public int EstadoBimestre { get; set; }
    public decimal Nota { get; set; }
    public Inscripciones Inscripciones { get; set; } = null!;
    public Areas Areas { get; set; } = null!;
}
