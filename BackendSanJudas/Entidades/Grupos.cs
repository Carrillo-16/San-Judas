using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSanJudas.Entidades;

public class Grupos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdGrupo { get; set; } // Generado automáticamente por SQL Server.
    [ForeignKey("Grados")]
    public int IdGrado { get; set; }

    [ForeignKey("CiclosEscolares")]
    public int IdCicloEscolar { get; set; }
    public int EstadoSeccion { get; set; }
    public int CupoMaximo { get; set; } = 25;
    public Grados Grados { get; set; } = null!;
    public CiclosEscolares CiclosEscolares { get; set; } = null!;
}
