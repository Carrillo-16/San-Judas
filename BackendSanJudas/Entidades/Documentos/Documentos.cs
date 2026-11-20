using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendSanJudas.Entidades;

public class Documentos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdDocumento { get; set; } // Generado automáticamente por SQL Server.
    public string Titulo { get; set; } = string.Empty;
    public int EstadoTipoDocumento { get; set; }
    [ForeignKey("Alumnos")]
    public int? IdAlumno { get; set; }
    [ForeignKey("Responsables")]
    public int? IdResponsable { get; set; }

    [ForeignKey("CategoriasDocumentos")]
    public int? IdCategoriaDocumento { get; set; }
    public DateOnly FechaDocumento { get; set; }
    public DateTime FechaCargaUtc { get; set; }
    public bool EsPlantilla { get; set; }
    public string RutaRelativa { get; set; } = string.Empty;
    public string NombreOriginal { get; set; } = string.Empty;
    public string TipoContenido { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public Alumnos? Alumnos { get; set; }
    public Responsables? Responsables { get; set; }
    public CategoriasDocumentos? CategoriasDocumentos { get; set; }
}
