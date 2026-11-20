using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BackendSanJudas.Entidades;

public class CategoriasDocumentos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCategoriaDocumento { get; set; } // Generado automáticamente por SQL Server.

    public string Nombre { get; set; } = string.Empty;
}
