using BackendSanJudas.Entidades;

using BackendSanJudas.Modelos.GradosAreas;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class GradosAreasServicio(DGDbContext contexto) : CrudServicio<GradosAreas,CrearGradosAreasDto,ActualizarGradosAreasDto,ConsultarGradosAreasDto>(contexto), IGradosAreasServicio
{
    protected override string NombreId => "IdGradoArea";
    protected override Expression<Func<GradosAreas,ConsultarGradosAreasDto>> Proyeccion => x=>new ConsultarGradosAreasDto { IdGradoArea=x.IdGradoArea, IdGrado=x.IdGrado, IdArea=x.IdArea, Orden=x.Orden };
    protected override void AplicarCrear(GradosAreas entidad,CrearGradosAreasDto modelo)
    {
        entidad.IdGrado=modelo.IdGrado;
        entidad.IdArea=modelo.IdArea;
        entidad.Orden=modelo.Orden;
    }
    protected override void AplicarActualizar(GradosAreas entidad,ActualizarGradosAreasDto modelo)
    {
        entidad.IdGrado=modelo.IdGrado;
        entidad.IdArea=modelo.IdArea;
        entidad.Orden=modelo.Orden;
    }
    protected override async Task ValidarAsync(GradosAreas entidad,bool nuevo,CancellationToken cancelacion)
    {

        if (!await Contexto.Grados.AnyAsync(x=>x.IdGrado==entidad.IdGrado,cancelacion) || !await Contexto.Areas.AnyAsync(x=>x.IdArea==entidad.IdArea,cancelacion)) throw new BadHttpRequestException("Grado o área no encontrado.",404);
        if (entidad.Orden<1 || entidad.Orden>30) throw new BadHttpRequestException("Orden no válido.");
        if (!nuevo && await TieneCalificacionesAsync(entidad,cancelacion)) throw new BadHttpRequestException("Esta asignación tiene calificaciones y no puede modificarse.",409);

    }

    private Task<bool> TieneCalificacionesAsync(GradosAreas entidad,CancellationToken cancelacion)
    {
        int grado=Contexto.Entry(entidad).OriginalValues.GetValue<int>(nameof(GradosAreas.IdGrado));
        int area=Contexto.Entry(entidad).OriginalValues.GetValue<int>(nameof(GradosAreas.IdArea));
        return (from nota in Contexto.Calificaciones join ins in Contexto.Inscripciones on nota.IdInscripcion equals ins.IdInscripcion join grupo in Contexto.Grupos on ins.IdGrupo equals grupo.IdGrupo where grupo.IdGrado==grado && nota.IdArea==area select nota).AnyAsync(cancelacion);
    }
    protected override async Task ValidarEliminarAsync(GradosAreas entidad,CancellationToken cancelacion)
    {
        if (await TieneCalificacionesAsync(entidad,cancelacion)) throw new BadHttpRequestException("La asignación tiene calificaciones y no puede eliminarse.",409);
    }
}
