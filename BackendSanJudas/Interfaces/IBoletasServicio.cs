using BackendSanJudas.Modelos.Boletas;
namespace BackendSanJudas.Interfaces;
public interface IBoletasServicio
{
    Task<BoletaDto> ObtenerAsync(int idInscripcion,CancellationToken cancelacion);
    Task<IReadOnlyList<BoletaDto>> ConsolidadoAsync(int idGrupo,CancellationToken cancelacion);
}
