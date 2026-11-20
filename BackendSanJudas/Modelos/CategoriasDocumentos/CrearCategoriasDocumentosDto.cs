using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.CategoriasDocumentos;

public class CrearCategoriasDocumentosDto
{
    [Required, StringLength(80)]
    public string Nombre { get; set; } = string.Empty;
}
