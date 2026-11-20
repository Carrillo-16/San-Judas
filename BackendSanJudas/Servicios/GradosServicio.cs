using BackendSanJudas.Entidades;
using BackendSanJudas.Modelos.Grados;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class GradosServicio(DGDbContext contexto) : CrudServicio<Grados,CrearGradosDto,ActualizarGradosDto,ConsultarGradosDto>(contexto), IGradosServicio
{
    protected override string NombreId => "IdGrado";
    protected override Expression<Func<Grados,ConsultarGradosDto>> Proyeccion => x=>new ConsultarGradosDto { IdGrado=x.IdGrado, Nombre=x.Nombre, EdadReferencia=x.EdadReferencia };
    protected override void AplicarCrear(Grados entidad,CrearGradosDto modelo)
    {
        entidad.Nombre = modelo.Nombre.Trim();
        entidad.EdadReferencia = modelo.EdadReferencia;
    }
    protected override void AplicarActualizar(Grados entidad,ActualizarGradosDto modelo)
    {
        entidad.Nombre = modelo.Nombre.Trim();
        entidad.EdadReferencia = modelo.EdadReferencia;
    }

}
