using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackendSanJudas.Entidades;

public class Areas
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdArea { get; set; } // Generado automáticamente por SQL Server.

    public string Nombre { get; set; } = string.Empty;
}
