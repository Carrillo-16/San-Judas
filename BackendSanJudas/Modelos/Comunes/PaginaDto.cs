namespace BackendSanJudas.Modelos.Comunes;
public class PaginaDto<T>
{
    public int Pagina { get; set; }
    public int TamanoPagina { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<T> Elementos { get; set; } = [];
}
