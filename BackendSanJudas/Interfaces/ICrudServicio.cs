using BackendSanJudas.Modelos.Comunes;
namespace BackendSanJudas.Interfaces;
public interface ICrudServicio<TCrear,TActualizar,TConsultar>
{
    Task<PaginaDto<TConsultar>> ListarAsync(PaginacionDto paginacion, CancellationToken cancelacion);
    Task<TConsultar> ObtenerAsync(int id, CancellationToken cancelacion);
    Task<TConsultar> CrearAsync(TCrear modelo, CancellationToken cancelacion);
    Task ActualizarAsync(int id, TActualizar modelo, CancellationToken cancelacion);
    Task EliminarAsync(int id, CancellationToken cancelacion);
}
