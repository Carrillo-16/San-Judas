using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.CategoriasDocumentos;

public class ConsultarCategoriasDocumentosDto
{
    public int IdCategoriaDocumento { get; set; }
    [Required, StringLength(80)]
    public string Nombre { get; set; } = string.Empty;
}
