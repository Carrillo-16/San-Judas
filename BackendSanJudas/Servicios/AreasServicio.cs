using BackendSanJudas.Entidades;
using BackendSanJudas.Modelos.Areas;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class AreasServicio(DGDbContext contexto) : CrudServicio<Areas,CrearAreasDto,ActualizarAreasDto,ConsultarAreasDto>(contexto), IAreasServicio
{
    protected override string NombreId => "IdArea";
    protected override Expression<Func<Areas,ConsultarAreasDto>> Proyeccion => x=>new ConsultarAreasDto { IdArea=x.IdArea, Nombre=x.Nombre };
    protected override void AplicarCrear(Areas entidad,CrearAreasDto modelo)
    {
        entidad.Nombre = modelo.Nombre.Trim();
    }
    protected override void AplicarActualizar(Areas entidad,ActualizarAreasDto modelo)
    {
        entidad.Nombre = modelo.Nombre.Trim();
    }

}
