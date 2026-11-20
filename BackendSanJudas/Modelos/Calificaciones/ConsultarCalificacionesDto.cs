
namespace BackendSanJudas.Modelos.Calificaciones;

public class ConsultarCalificacionesDto
{
    public int IdCalificacion { get; set; }
    public int IdInscripcion { get; set; }
    public int IdArea { get; set; }
    public int EstadoBimestre { get; set; }
    public decimal Nota { get; set; }
}
