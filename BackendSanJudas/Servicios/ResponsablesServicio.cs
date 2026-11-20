using BackendSanJudas.Entidades;
using BackendSanJudas.Modelos.Responsables;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class ResponsablesServicio(DGDbContext contexto) : CrudServicio<Responsables,CrearResponsablesDto,ActualizarResponsablesDto,ConsultarResponsablesDto>(contexto), IResponsablesServicio
{
    protected override string NombreId => "IdResponsable";
    protected override Expression<Func<Responsables,ConsultarResponsablesDto>> Proyeccion => x=>new ConsultarResponsablesDto { IdResponsable=x.IdResponsable, NombreCompleto=x.NombreCompleto, FechaNacimiento=x.FechaNacimiento, Dpi=x.Dpi, Telefono=x.Telefono };
    protected override void AplicarCrear(Responsables entidad,CrearResponsablesDto modelo)
    {
        entidad.NombreCompleto = modelo.NombreCompleto.Trim();
        entidad.FechaNacimiento = modelo.FechaNacimiento;
        entidad.Dpi = modelo.Dpi?.Trim();
        entidad.Telefono = modelo.Telefono.Trim();
    }
    protected override void AplicarActualizar(Responsables entidad,ActualizarResponsablesDto modelo)
    {
        entidad.NombreCompleto = modelo.NombreCompleto.Trim();
        entidad.FechaNacimiento = modelo.FechaNacimiento;
        entidad.Dpi = modelo.Dpi?.Trim();
        entidad.Telefono = modelo.Telefono.Trim();
    }

    protected override async Task ValidarAsync(Responsables entidad,bool nuevo,CancellationToken cancelacion)
    {
        if (entidad.FechaNacimiento is not null && (entidad.FechaNacimiento==default(DateOnly) || entidad.FechaNacimiento>BackendSanJudas.Servicios.RelojEscolar.Hoy)) throw new BadHttpRequestException("La fecha de nacimiento no es válida.");
        if (string.IsNullOrWhiteSpace(entidad.Dpi)) entidad.Dpi=null;
        var contactos=await (from inscripcion in Contexto.Inscripciones join vinculo in Contexto.AlumnosResponsables on new { inscripcion.IdAlumno, IdResponsable=inscripcion.IdEncargado } equals new { vinculo.IdAlumno, vinculo.IdResponsable } where inscripcion.IdEncargado==entidad.IdResponsable && (vinculo.EstadoParentesco==2 || vinculo.EstadoParentesco==1) select inscripcion.TelefonoEncargado).ToListAsync(cancelacion);
        if (contactos.Any(t=>ReglasInscripcion.NormalizarTelefono(t)==ReglasInscripcion.NormalizarTelefono(entidad.Telefono))) throw new BadHttpRequestException("El teléfono del responsable debe ser distinto del contacto adicional de encargado.");
    }
}
