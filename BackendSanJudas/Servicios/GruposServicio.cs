using BackendSanJudas.Entidades;

using BackendSanJudas.Modelos.Grupos;
using BackendSanJudas.Interfaces;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace BackendSanJudas.Servicios;
public class GruposServicio(DGDbContext contexto) : CrudServicio<Grupos,CrearGruposDto,ActualizarGruposDto,ConsultarGruposDto>(contexto), IGruposServicio
{
    protected override string NombreId => "IdGrupo";
    protected override Expression<Func<Grupos,ConsultarGruposDto>> Proyeccion => x=>new ConsultarGruposDto { IdGrupo=x.IdGrupo, IdGrado=x.IdGrado, IdCicloEscolar=x.IdCicloEscolar, EstadoSeccion=x.EstadoSeccion, CupoMaximo=x.CupoMaximo };
    protected override void AplicarCrear(Grupos entidad,CrearGruposDto modelo)
    {
        entidad.IdGrado=modelo.IdGrado;
        entidad.IdCicloEscolar=modelo.IdCicloEscolar;
        entidad.EstadoSeccion=modelo.EstadoSeccion;
        entidad.CupoMaximo=modelo.CupoMaximo;
    }
    protected override void AplicarActualizar(Grupos entidad,ActualizarGruposDto modelo)
    {
        if (entidad.IdCicloEscolar!=modelo.IdCicloEscolar) throw new BadHttpRequestException("El ciclo de un grupo no se puede modificar. Cree un grupo para el nuevo ciclo.",409);
        entidad.IdGrado=modelo.IdGrado;
        entidad.IdCicloEscolar=modelo.IdCicloEscolar;
        entidad.EstadoSeccion=modelo.EstadoSeccion;
        entidad.CupoMaximo=modelo.CupoMaximo;
    }
    protected override async Task ValidarAsync(Grupos entidad,bool nuevo,CancellationToken cancelacion)
    {

        if ((entidad.EstadoSeccion<1 || entidad.EstadoSeccion>2) || entidad.CupoMaximo<1 || entidad.CupoMaximo>100) throw new BadHttpRequestException("Sección o cupo no válido.");
        if (!await Contexto.Grados.AnyAsync(x=>x.IdGrado==entidad.IdGrado,cancelacion)) throw new BadHttpRequestException("Grado no encontrado.",404);
        var ciclo=await Contexto.CiclosEscolares.FindAsync([entidad.IdCicloEscolar],cancelacion) ?? throw new BadHttpRequestException("Ciclo no encontrado.",404);
        if (ciclo.Cerrado) throw new BadHttpRequestException("El ciclo está cerrado.",409);
        if (!nuevo)
        {
            var original=Contexto.Entry(entidad).OriginalValues;
            bool tieneInscripciones=await Contexto.Inscripciones.AnyAsync(x=>x.IdGrupo==entidad.IdGrupo,cancelacion);
            if (tieneInscripciones && ((int)original[nameof(Grupos.IdGrado)]! != entidad.IdGrado || (int)original[nameof(Grupos.IdCicloEscolar)]! != entidad.IdCicloEscolar || (int)original[nameof(Grupos.EstadoSeccion)]! != entidad.EstadoSeccion)) throw new BadHttpRequestException("Un grupo con inscripciones no puede cambiar de grado, sección ni ciclo.",409);
            int ocupados=await Contexto.Inscripciones.CountAsync(x=>x.IdGrupo==entidad.IdGrupo,cancelacion);
            if (entidad.CupoMaximo<ocupados) throw new BadHttpRequestException("El cupo no puede ser menor que los alumnos inscritos.",409);
        }

    }

}
