using BackendSanJudas.Entidades;
using BackendSanJudas.Modelos.AlumnosResponsables;
namespace BackendSanJudas.Interfaces;
public interface IAlumnosResponsablesServicio
{
    Task<IReadOnlyList<ConsultarAlumnoResponsableDto>> ListarAsync(int idAlumno,CancellationToken cancelacion);
    Task<ConsultarAlumnoResponsableDto> VincularAsync(int idAlumno,VincularResponsableDto modelo,CancellationToken cancelacion);
    Task DesvincularAsync(int idAlumno,int idResponsable,CancellationToken cancelacion);
}
