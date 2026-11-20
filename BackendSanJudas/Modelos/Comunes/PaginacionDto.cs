using System.ComponentModel.DataAnnotations;

namespace BackendSanJudas.Modelos.Comunes;

public class PaginacionDto
{
    [Range(1,int.MaxValue)] public int Pagina { get; set; } = 1;
    [Range(1,100)] public int TamanoPagina { get; set; } = 25;
}
