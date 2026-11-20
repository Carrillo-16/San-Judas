using BackendSanJudas.Entidades;

using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.AlumnosResponsables;
using Microsoft.EntityFrameworkCore;
namespace BackendSanJudas.Servicios;
public class AlumnosResponsablesServicio(DGDbContext contexto) : IAlumnosResponsablesServicio
{
    public async Task<IReadOnlyList<ConsultarAlumnoResponsableDto>> ListarAsync(int idAlumno,CancellationToken cancelacion)
    {
        if (!await contexto.Alumnos.AnyAsync(x=>x.IdAlumno==idAlumno,cancelacion)) throw new BadHttpRequestException("Alumno no encontrado.",404);
        return await (from vinculo in contexto.AlumnosResponsables.AsNoTracking() join persona in contexto.Responsables on vinculo.IdResponsable equals persona.IdResponsable where vinculo.IdAlumno==idAlumno orderby vinculo.EstadoParentesco select new ConsultarAlumnoResponsableDto { IdAlumnoResponsable=vinculo.IdAlumnoResponsable, IdAlumno=vinculo.IdAlumno, IdResponsable=vinculo.IdResponsable, EstadoParentesco=vinculo.EstadoParentesco, NombreCompleto=persona.NombreCompleto, FechaNacimiento=persona.FechaNacimiento, Dpi=persona.Dpi, Telefono=persona.Telefono }).ToListAsync(cancelacion);
    }
    public async Task<ConsultarAlumnoResponsableDto> VincularAsync(int idAlumno,VincularResponsableDto modelo,CancellationToken cancelacion)
    {
        if ((modelo.EstadoParentesco<1 || modelo.EstadoParentesco>3)) throw new BadHttpRequestException("EstadoParentesco no válido.");
        if (!await contexto.Alumnos.AnyAsync(x=>x.IdAlumno==idAlumno,cancelacion) || !await contexto.Responsables.AnyAsync(x=>x.IdResponsable==modelo.IdResponsable,cancelacion)) throw new BadHttpRequestException("Alumno o responsable no encontrado.",404);
        var relacion=new AlumnosResponsables { IdAlumno=idAlumno, IdResponsable=modelo.IdResponsable, EstadoParentesco=modelo.EstadoParentesco };
        contexto.Add(relacion); await ValidacionesRegistro.GuardarAsync(contexto,cancelacion); await contexto.SaveChangesAsync(cancelacion); return (await ListarAsync(idAlumno,cancelacion)).Single(x=>x.IdResponsable==modelo.IdResponsable);
    }
    public async Task DesvincularAsync(int idAlumno,int idResponsable,CancellationToken cancelacion)
    {
        var relacion=await contexto.AlumnosResponsables.SingleOrDefaultAsync(x=>x.IdAlumno==idAlumno && x.IdResponsable==idResponsable,cancelacion) ?? throw new BadHttpRequestException("Relación no encontrada.",404);
        if (await contexto.Documentos.AnyAsync(x=>x.IdAlumno==idAlumno && x.IdResponsable==idResponsable,cancelacion)) throw new BadHttpRequestException("El responsable tiene documentos asociados a este alumno.",409);
        await ValidacionesRegistro.EliminarAsync(contexto,relacion,cancelacion); if(await contexto.Inscripciones.AnyAsync(i=>i.IdAlumno==idAlumno && i.IdEncargado==idResponsable,cancelacion)) throw new BadHttpRequestException("Esta persona es encargada de una inscripción. Cambie el encargado antes de desvincularla.",409);
        contexto.Remove(relacion); await ValidacionesRegistro.GuardarAsync(contexto,cancelacion); await contexto.SaveChangesAsync(cancelacion);
    }
}
