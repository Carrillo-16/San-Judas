using BackendSanJudas.Entidades;
using BackendSanJudas.Modelos.CiclosEscolares;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class CiclosEscolaresServicio(DGDbContext contexto) : CrudServicio<CiclosEscolares,CrearCiclosEscolaresDto,ActualizarCiclosEscolaresDto,ConsultarCiclosEscolaresDto>(contexto), ICiclosEscolaresServicio
{
    protected override string NombreId => "IdCicloEscolar";
    protected override Expression<Func<CiclosEscolares,ConsultarCiclosEscolaresDto>> Proyeccion => x=>new ConsultarCiclosEscolaresDto { IdCicloEscolar=x.IdCicloEscolar, Anio=x.Anio, Cerrado=x.Cerrado };
    protected override void AplicarCrear(CiclosEscolares entidad,CrearCiclosEscolaresDto modelo)
    {
        entidad.Anio = modelo.Anio;
        entidad.Cerrado = modelo.Cerrado;
    }
    protected override void AplicarActualizar(CiclosEscolares entidad,ActualizarCiclosEscolaresDto modelo)
    {
        if (entidad.Anio!=modelo.Anio && Contexto.Grupos.Any(x=>x.IdCicloEscolar==entidad.IdCicloEscolar)) throw new BadHttpRequestException("No puede cambiar el año de un ciclo con grupos registrados.",409);
        entidad.Anio = modelo.Anio;
        entidad.Cerrado = modelo.Cerrado;
    }

}
