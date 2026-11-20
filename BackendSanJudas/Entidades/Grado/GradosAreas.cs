using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSanJudas.Entidades;

public class GradosAreas
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdGradoArea { get; set; } // Generado automáticamente por SQL Server.
    [ForeignKey("Grados")]
    public int IdGrado { get; set; }

    [ForeignKey("Areas")]
    public int IdArea { get; set; }
    public int Orden { get; set; }
    public Grados Grados { get; set; } = null!;
    public Areas Areas { get; set; } = null!;
}
