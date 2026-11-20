using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.Calificaciones;

public class CrearCalificacionesDto
{
    [Range(1,int.MaxValue)] public int IdInscripcion { get; set; }
    [Range(1,int.MaxValue)] public int IdArea { get; set; }
    [Range(1,4)] public int EstadoBimestre { get; set; }
    [Range(typeof(decimal),"0","100")] public decimal Nota { get; set; }
}
