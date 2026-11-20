using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackendSanJudas.Entidades;

public class CiclosEscolares
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCicloEscolar { get; set; } // Generado automáticamente por SQL Server.

    public int Anio { get; set; }

    public bool Cerrado { get; set; }
}
