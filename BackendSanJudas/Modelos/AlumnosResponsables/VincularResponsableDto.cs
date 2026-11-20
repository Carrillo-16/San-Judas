using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.AlumnosResponsables;

public class VincularResponsableDto
{
    [Range(1,int.MaxValue)] public int IdResponsable { get; set; }
    [Range(1,3)] public int EstadoParentesco { get; set; }
}
