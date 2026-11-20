using BackendSanJudas.Entidades;
using BackendSanJudas.Modelos.CategoriasDocumentos;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class CategoriasDocumentosServicio(DGDbContext contexto) : CrudServicio<CategoriasDocumentos,CrearCategoriasDocumentosDto,ActualizarCategoriasDocumentosDto,ConsultarCategoriasDocumentosDto>(contexto), ICategoriasDocumentosServicio
{
    protected override string NombreId => "IdCategoriaDocumento";
    protected override Expression<Func<CategoriasDocumentos,ConsultarCategoriasDocumentosDto>> Proyeccion => x=>new ConsultarCategoriasDocumentosDto { IdCategoriaDocumento=x.IdCategoriaDocumento, Nombre=x.Nombre };
    protected override void AplicarCrear(CategoriasDocumentos entidad,CrearCategoriasDocumentosDto modelo)
    {
        entidad.Nombre = modelo.Nombre.Trim();
    }
    protected override void AplicarActualizar(CategoriasDocumentos entidad,ActualizarCategoriasDocumentosDto modelo)
    {
        entidad.Nombre = modelo.Nombre.Trim();
    }

}
