using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.Documentos;

public class ActualizarDocumentoDto
{
    [Required, StringLength(200)] public string Titulo { get; set; } = string.Empty;
    public DateOnly FechaDocumento { get; set; }
    public int? IdCategoriaDocumento { get; set; }
    public bool EsPlantilla { get; set; }
}
