using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.GradosAreas;

public class CrearGradosAreasDto
{
    [Range(1,int.MaxValue)] public int IdGrado { get; set; }
    [Range(1,int.MaxValue)] public int IdArea { get; set; }
    [Range(1,30)] public int Orden { get; set; }
}
