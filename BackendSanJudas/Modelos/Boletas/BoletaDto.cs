namespace BackendSanJudas.Modelos.Boletas;
public class BoletaDto
{
    public int IdInscripcion { get; set; }
    public int IdAlumno { get; set; }
    public string NombreAlumno { get; set; } = "";
    public string CodigoAlumno { get; set; } = "";
    public string Grado { get; set; } = "";
    public int EstadoSeccion { get; set; }
    public int Anio { get; set; }
    public IReadOnlyList<AreaBoletaDto> Areas { get; set; } = [];
}
