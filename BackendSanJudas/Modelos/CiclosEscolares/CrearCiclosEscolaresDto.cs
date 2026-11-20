using System.ComponentModel.DataAnnotations;
namespace BackendSanJudas.Modelos.CiclosEscolares;

public class CrearCiclosEscolaresDto
{
    [Range(2000,2200)]
    public int Anio { get; set; }

    public bool Cerrado { get; set; }
}
