using BackendSanJudas.Entidades;
using BackendSanJudas.Modelos.Alumnos;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class AlumnosServicio(DGDbContext contexto) : CrudServicio<Alumnos,CrearAlumnosDto,ActualizarAlumnosDto,ConsultarAlumnosDto>(contexto), IAlumnosServicio
{
    protected override string NombreId => "IdAlumno";
    protected override Expression<Func<Alumnos,ConsultarAlumnosDto>> Proyeccion => x=>new ConsultarAlumnosDto { IdAlumno=x.IdAlumno, NombresAlumno=x.NombresAlumno, ApellidosAlumno=x.ApellidosAlumno, CodigoAlumno=x.CodigoAlumno, Genero=x.Genero, FechaNacimiento=x.FechaNacimiento, TieneAdecuacionCurricular=x.TieneAdecuacionCurricular, FotoUrl=Contexto.Documentos.Where(d=>d.IdAlumno==x.IdAlumno && d.EstadoTipoDocumento==1).Select(d=>"/Api/Documentos/"+d.IdDocumento+"/Archivo").FirstOrDefault() };
    protected override void AplicarCrear(Alumnos entidad,CrearAlumnosDto modelo)
    {
        entidad.NombresAlumno = modelo.NombresAlumno.Trim();
        entidad.ApellidosAlumno = modelo.ApellidosAlumno.Trim();
        entidad.CodigoAlumno = modelo.CodigoAlumno.Trim();
        entidad.Genero = string.IsNullOrWhiteSpace(modelo.Genero) ? null : modelo.Genero.Trim();
        entidad.FechaNacimiento = modelo.FechaNacimiento;
        entidad.TieneAdecuacionCurricular = modelo.TieneAdecuacionCurricular;
    }
    protected override void AplicarActualizar(Alumnos entidad,ActualizarAlumnosDto modelo)
    {
        entidad.NombresAlumno = modelo.NombresAlumno.Trim();
        entidad.ApellidosAlumno = modelo.ApellidosAlumno.Trim();
        entidad.CodigoAlumno = modelo.CodigoAlumno.Trim();
        entidad.Genero = string.IsNullOrWhiteSpace(modelo.Genero) ? null : modelo.Genero.Trim();
        entidad.FechaNacimiento = modelo.FechaNacimiento;
        entidad.TieneAdecuacionCurricular = modelo.TieneAdecuacionCurricular;
    }

    protected override async Task ValidarAsync(Alumnos entidad,bool nuevo,CancellationToken cancelacion)
    {
        if (entidad.FechaNacimiento==default || entidad.FechaNacimiento>BackendSanJudas.Servicios.RelojEscolar.Hoy) throw new BadHttpRequestException("La fecha de nacimiento no es válida.");
        entidad.CodigoAlumno=entidad.CodigoAlumno.ToUpperInvariant();
        if (entidad.Genero is not (null or "Masculino" or "Femenino")) throw new BadHttpRequestException("Género no válido.");
        if (await Contexto.Inscripciones.AnyAsync(x=>x.IdAlumno==entidad.IdAlumno && x.FechaInscripcion<entidad.FechaNacimiento,cancelacion)) throw new BadHttpRequestException("El nacimiento no puede ser posterior a una inscripción existente.");
    }
}
