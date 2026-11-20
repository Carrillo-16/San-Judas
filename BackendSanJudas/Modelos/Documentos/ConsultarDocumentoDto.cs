using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.Documentos;

public class ConsultarDocumentoDto
{
    public int IdDocumento { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int EstadoTipoDocumento { get; set; }
    public int? IdAlumno { get; set; }
    public int? IdResponsable { get; set; }
    public int? IdCategoriaDocumento { get; set; }
    public DateOnly FechaDocumento { get; set; }
    public DateTime FechaCargaUtc { get; set; }
    public bool EsPlantilla { get; set; }
    public string NombreOriginal { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public string UrlDescarga { get; set; } = string.Empty;
}
