using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackendSanJudas.Entidades;

public class Grados
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdGrado { get; set; } // Generado automáticamente por SQL Server.

    public string Nombre { get; set; } = string.Empty;

    public int? EdadReferencia { get; set; }
}
