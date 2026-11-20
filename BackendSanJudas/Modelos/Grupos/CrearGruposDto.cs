using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.Grupos;

public class CrearGruposDto
{
    [Range(1,int.MaxValue)] public int IdGrado { get; set; }
    [Range(1,int.MaxValue)] public int IdCicloEscolar { get; set; }
    [Range(1,2)] public int EstadoSeccion { get; set; }
    [Range(1,100)] public int CupoMaximo { get; set; } = 25;
}
