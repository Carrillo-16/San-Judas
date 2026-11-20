using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.Documentos;

public class SubirDocumentoDto
{
    [Required, StringLength(200)] public string Titulo { get; set; } = string.Empty;
    [Range(1,5)] public int EstadoTipoDocumento { get; set; }
    public int? IdAlumno { get; set; }
    public int? IdResponsable { get; set; }
    public int? IdCategoriaDocumento { get; set; }
    public DateOnly FechaDocumento { get; set; }
    public bool EsPlantilla { get; set; }
    [Required] public IFormFile Archivo { get; set; } = null!;
}
