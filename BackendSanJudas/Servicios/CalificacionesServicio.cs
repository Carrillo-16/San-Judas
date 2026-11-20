using BackendSanJudas.Entidades;

using BackendSanJudas.Modelos.Calificaciones;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class CalificacionesServicio(DGDbContext contexto) : CrudServicio<Calificaciones,CrearCalificacionesDto,ActualizarCalificacionesDto,ConsultarCalificacionesDto>(contexto), ICalificacionesServicio
{
    protected override string NombreId => "IdCalificacion";
    protected override Expression<Func<Calificaciones,ConsultarCalificacionesDto>> Proyeccion => x=>new ConsultarCalificacionesDto { IdCalificacion=x.IdCalificacion, IdInscripcion=x.IdInscripcion, IdArea=x.IdArea, EstadoBimestre=x.EstadoBimestre, Nota=x.Nota };
    protected override void AplicarCrear(Calificaciones entidad,CrearCalificacionesDto modelo)
    {
        entidad.IdInscripcion=modelo.IdInscripcion;
        entidad.IdArea=modelo.IdArea;
        entidad.EstadoBimestre=modelo.EstadoBimestre;
        entidad.Nota=modelo.Nota;
    }
    protected override void AplicarActualizar(Calificaciones entidad,ActualizarCalificacionesDto modelo)
    {
        entidad.IdInscripcion=modelo.IdInscripcion;
        entidad.IdArea=modelo.IdArea;
        entidad.EstadoBimestre=modelo.EstadoBimestre;
        entidad.Nota=modelo.Nota;
    }
    protected override async Task ValidarAsync(Calificaciones entidad,bool nuevo,CancellationToken cancelacion)
    {

        if ((entidad.EstadoBimestre<1 || entidad.EstadoBimestre>4) || entidad.Nota<0 || entidad.Nota>100 || decimal.Round(entidad.Nota,2)!=entidad.Nota) throw new BadHttpRequestException("EstadoBimestre o nota no válida. Utilice hasta dos decimales, de 0 a 100.");
        var inscripcion=await Contexto.Inscripciones.FindAsync([entidad.IdInscripcion],cancelacion) ?? throw new BadHttpRequestException("Inscripción no encontrada.",404);
        var grupo=await Contexto.Grupos.FindAsync([inscripcion.IdGrupo],cancelacion);
        if (await Contexto.CiclosEscolares.AnyAsync(x=>x.IdCicloEscolar==grupo!.IdCicloEscolar && x.Cerrado,cancelacion)) throw new BadHttpRequestException("El ciclo está cerrado.",409);
        if (!await Contexto.GradosAreas.AnyAsync(x=>x.IdGrado==grupo!.IdGrado && x.IdArea==entidad.IdArea,cancelacion)) throw new BadHttpRequestException("El área no corresponde al grado de esta inscripción.");
        if (!nuevo)
        {
            var original=Contexto.Entry(entidad).OriginalValues;
            if ((int)original[nameof(Calificaciones.IdInscripcion)]! != entidad.IdInscripcion || (int)original[nameof(Calificaciones.IdArea)]! != entidad.IdArea || (int)original[nameof(Calificaciones.EstadoBimestre)]! != entidad.EstadoBimestre) throw new BadHttpRequestException("Solo se puede modificar la nota. Para cambiar alumno, área o bimestre, elimine el registro y créelo nuevamente.");
        }

    }

    protected override async Task ValidarEliminarAsync(Calificaciones entidad,CancellationToken cancelacion)
    {
        int ciclo=Contexto.Inscripciones.Where(i=>i.IdInscripcion==entidad.IdInscripcion).Select(i=>i.IdCicloEscolar).First();
        if (await Contexto.CiclosEscolares.AnyAsync(x=>x.IdCicloEscolar==ciclo && x.Cerrado,cancelacion)) throw new BadHttpRequestException("El ciclo está cerrado.",409);
    }
}
