using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.CategoriasDocumentos;

public class ActualizarCategoriasDocumentosDto
{
    [Required, StringLength(80)]
    public string Nombre { get; set; } = string.Empty;
}
