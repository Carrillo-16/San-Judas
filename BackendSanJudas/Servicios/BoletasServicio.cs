using BackendSanJudas.Entidades;


using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.Boletas;
using Microsoft.EntityFrameworkCore;
namespace BackendSanJudas.Servicios;
public class BoletasServicio(DGDbContext contexto) : IBoletasServicio
{
    public async Task<BoletaDto> ObtenerAsync(int idInscripcion,CancellationToken cancelacion)
    {
        var inscripcion=await contexto.Inscripciones.AsNoTracking().SingleOrDefaultAsync(i=>i.IdInscripcion==idInscripcion,cancelacion) ?? throw new BadHttpRequestException("Inscripción no encontrada.",404);
        return (await ConsolidadoAsync(inscripcion.IdGrupo,cancelacion)).Single(x=>x.IdInscripcion==idInscripcion);
    }
    public async Task<IReadOnlyList<BoletaDto>> ConsolidadoAsync(int idGrupo,CancellationToken cancelacion)
    {
        var grupo=await contexto.Grupos.AsNoTracking().SingleOrDefaultAsync(x=>x.IdGrupo==idGrupo,cancelacion) ?? throw new BadHttpRequestException("Grupo no encontrado.",404);
        var grado=await contexto.Grados.AsNoTracking().SingleAsync(x=>x.IdGrado==grupo.IdGrado,cancelacion);
        int anio=await contexto.CiclosEscolares.Where(x=>x.IdCicloEscolar==grupo.IdCicloEscolar).Select(x=>x.Anio).SingleAsync(cancelacion);
        var areas=await (from asignacion in contexto.GradosAreas.AsNoTracking() join area in contexto.Areas on asignacion.IdArea equals area.IdArea where asignacion.IdGrado==grupo.IdGrado orderby asignacion.Orden select new { area.IdArea, area.Nombre, asignacion.Orden }).ToListAsync(cancelacion);
        var alumnos=await (from inscripcion in contexto.Inscripciones.AsNoTracking() join alumno in contexto.Alumnos on inscripcion.IdAlumno equals alumno.IdAlumno where inscripcion.IdGrupo==idGrupo orderby alumno.ApellidosAlumno,alumno.NombresAlumno select new { inscripcion.IdInscripcion, alumno.IdAlumno, alumno.NombresAlumno, alumno.ApellidosAlumno, alumno.CodigoAlumno }).ToListAsync(cancelacion);
        var notas=await (from nota in contexto.Calificaciones.AsNoTracking() join inscripcion in contexto.Inscripciones on nota.IdInscripcion equals inscripcion.IdInscripcion where inscripcion.IdGrupo==idGrupo select nota).ToListAsync(cancelacion);
        var indice=notas.ToDictionary(n=>(n.IdInscripcion,n.IdArea,n.EstadoBimestre),n=>n.Nota);
        return alumnos.Select(alumno=>new BoletaDto { IdInscripcion=alumno.IdInscripcion, IdAlumno=alumno.IdAlumno, NombreAlumno=$"{alumno.NombresAlumno} {alumno.ApellidosAlumno}", CodigoAlumno=alumno.CodigoAlumno, Grado=grado.Nombre, EstadoSeccion=grupo.EstadoSeccion, Anio=anio,
            Areas=areas.Select(area=>new AreaBoletaDto { IdArea=area.IdArea, Nombre=area.Nombre, Orden=area.Orden,
                I=indice.TryGetValue((alumno.IdInscripcion,area.IdArea,1),out var i)?i:null,
                II=indice.TryGetValue((alumno.IdInscripcion,area.IdArea,2),out var ii)?ii:null,
                III=indice.TryGetValue((alumno.IdInscripcion,area.IdArea,3),out var iii)?iii:null,
                IV=indice.TryGetValue((alumno.IdInscripcion,area.IdArea,4),out var iv)?iv:null }).ToList() }).ToList();
    }
}
