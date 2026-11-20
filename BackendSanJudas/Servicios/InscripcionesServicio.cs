using BackendSanJudas.Entidades;

using BackendSanJudas.Modelos.Inscripciones;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class InscripcionesServicio(DGDbContext contexto) : CrudServicio<Inscripciones,CrearInscripcionesDto,ActualizarInscripcionesDto,ConsultarInscripcionesDto>(contexto), IInscripcionesServicio
{
    protected override string NombreId => "IdInscripcion";
    protected override Expression<Func<Inscripciones,ConsultarInscripcionesDto>> Proyeccion => x=>new ConsultarInscripcionesDto { IdInscripcion=x.IdInscripcion, IdAlumno=x.IdAlumno, IdGrupo=x.IdGrupo, FechaInscripcion=x.FechaInscripcion, IdEncargado=x.IdEncargado, TelefonoEncargado=x.TelefonoEncargado, IdCicloEscolar=x.IdCicloEscolar };
    protected override void AplicarCrear(Inscripciones entidad,CrearInscripcionesDto modelo)
    {
        entidad.IdAlumno=modelo.IdAlumno;
        entidad.IdGrupo=modelo.IdGrupo;
        entidad.FechaInscripcion=modelo.FechaInscripcion;
        entidad.IdEncargado=modelo.IdEncargado;
        entidad.TelefonoEncargado=modelo.TelefonoEncargado;
    }
    protected override void AplicarActualizar(Inscripciones entidad,ActualizarInscripcionesDto modelo)
    {
        entidad.IdAlumno=modelo.IdAlumno;
        entidad.IdGrupo=modelo.IdGrupo;
        entidad.FechaInscripcion=modelo.FechaInscripcion;
        entidad.IdEncargado=modelo.IdEncargado;
        entidad.TelefonoEncargado=modelo.TelefonoEncargado;
    }
    protected override async Task ValidarAsync(Inscripciones entidad,bool nuevo,CancellationToken cancelacion)
    {

        var grupo=await Contexto.Grupos.FindAsync([entidad.IdGrupo],cancelacion) ?? throw new BadHttpRequestException("Grupo no encontrado.",404);
        var ciclo=await Contexto.CiclosEscolares.FindAsync([grupo.IdCicloEscolar],cancelacion) ?? throw new BadHttpRequestException("Ciclo no encontrado.",404);
        if (ciclo.Cerrado) throw new BadHttpRequestException("El ciclo está cerrado.",409);
        var alumno=await Contexto.Alumnos.FindAsync([entidad.IdAlumno],cancelacion) ?? throw new BadHttpRequestException("Alumno no encontrado.",404);
        if (entidad.FechaInscripcion==default || entidad.FechaInscripcion.Year!=ciclo.Anio || entidad.FechaInscripcion<alumno.FechaNacimiento) throw new BadHttpRequestException("La fecha de inscripción debe pertenecer al ciclo y ser posterior al nacimiento.");
        var vinculo=await Contexto.AlumnosResponsables.SingleOrDefaultAsync(x=>x.IdAlumno==entidad.IdAlumno && x.IdResponsable==entidad.IdEncargado,cancelacion) ?? throw new BadHttpRequestException("El encargado debe estar vinculado al alumno.");
        var encargado=await Contexto.Responsables.FindAsync([vinculo.IdResponsable],cancelacion);
        entidad.TelefonoEncargado=entidad.TelefonoEncargado.Trim();
        if (vinculo.EstadoParentesco is 1 or 2) ReglasInscripcion.ValidarTelefono(encargado!.Telefono,entidad.TelefonoEncargado);
        if (!nuevo)
        {
            var original=Contexto.Entry(entidad).OriginalValues;
            int cicloOriginal=(int)original[nameof(Inscripciones.IdCicloEscolar)]!;
            if (cicloOriginal!=grupo.IdCicloEscolar || (int)original[nameof(Inscripciones.IdAlumno)]! != entidad.IdAlumno) throw new BadHttpRequestException("No puede cambiar el alumno ni el ciclo de una inscripción.",409);
            if ((int)original[nameof(Inscripciones.IdGrupo)]! != entidad.IdGrupo && await Contexto.Calificaciones.AnyAsync(x=>x.IdInscripcion==entidad.IdInscripcion,cancelacion)) throw new BadHttpRequestException("No puede cambiar el grupo de una inscripción con calificaciones.",409);
        }
        entidad.IdCicloEscolar=grupo.IdCicloEscolar;
        int ocupados=await Contexto.Inscripciones.CountAsync(x=>x.IdGrupo==grupo.IdGrupo && x.IdInscripcion!=entidad.IdInscripcion,cancelacion);
        ReglasInscripcion.ValidarCupo(ocupados,grupo.CupoMaximo);

    }

    protected override async Task ValidarEliminarAsync(Inscripciones entidad,CancellationToken cancelacion)
    {
        int ciclo=entidad.IdCicloEscolar;
        if (await Contexto.CiclosEscolares.AnyAsync(x=>x.IdCicloEscolar==ciclo && x.Cerrado,cancelacion)) throw new BadHttpRequestException("El ciclo está cerrado.",409);
    }
}
