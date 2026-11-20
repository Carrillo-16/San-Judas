
namespace BackendSanJudas.Modelos.Grupos;

public class ConsultarGruposDto
{
    public int IdGrupo { get; set; }
    public int IdGrado { get; set; }
    public int IdCicloEscolar { get; set; }
    public int EstadoSeccion { get; set; }
    public int CupoMaximo { get; set; } = 25;
}
