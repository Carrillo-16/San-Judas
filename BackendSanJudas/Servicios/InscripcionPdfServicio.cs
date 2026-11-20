using BackendSanJudas.Entidades;
using BackendSanJudas.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace BackendSanJudas.Servicios;
public class InscripcionPdfServicio(DGDbContext contexto,PlantillaAlumnoPdf plantilla,IAlmacenamientoArchivos archivos) : IInscripcionPdfServicio
{
    public async Task<byte[]> GenerarAsync(int idInscripcion,CancellationToken ct)
    {
        var ins=await contexto.Inscripciones.AsNoTracking().SingleOrDefaultAsync(x=>x.IdInscripcion==idInscripcion,ct) ?? throw new BadHttpRequestException("Inscripción no encontrada.",404);
        var alumno=await contexto.Alumnos.AsNoTracking().SingleAsync(x=>x.IdAlumno==ins.IdAlumno,ct);
        var grupo=await contexto.Grupos.AsNoTracking().SingleAsync(x=>x.IdGrupo==ins.IdGrupo,ct);
        var grado=await contexto.Grados.AsNoTracking().SingleAsync(x=>x.IdGrado==grupo.IdGrado,ct);
        var ciclo=await contexto.CiclosEscolares.AsNoTracking().SingleAsync(x=>x.IdCicloEscolar==ins.IdCicloEscolar,ct);
        var personas=await (from v in contexto.AlumnosResponsables join p in contexto.Responsables on v.IdResponsable equals p.IdResponsable where v.IdAlumno==alumno.IdAlumno select new {v.EstadoParentesco,Persona=p}).ToListAsync(ct);
        string Edad(DateOnly? fecha) { if(fecha==null)return ""; int edad=ins.FechaInscripcion.Year-fecha.Value.Year; if(fecha.Value.AddYears(edad)>ins.FechaInscripcion)edad--; return edad+" años"; }
        var datos=new Dictionary<string,string> {
            ["C5"]=ins.FechaInscripcion.ToString("dd/MM/yyyy"),["C6"]=ciclo.Anio.ToString(),
            ["C10"]=alumno.NombresAlumno+" "+alumno.ApellidosAlumno,["B11"]=alumno.CodigoAlumno,["C12"]=alumno.FechaNacimiento.ToString("dd/MM/yyyy"),
            ["B13"]=grado.Nombre,["D13"]=grupo.EstadoSeccion==1?"A":"B",["B14"]=alumno.Genero??"",["D14"]=Edad(alumno.FechaNacimiento),["F10"]=""
        };
        void Padre(Responsables? p,int fila) {
            datos[$"C{fila}"]=p?.NombreCompleto??"";datos[$"F{fila}"]=p?.Dpi??"";
            datos[$"C{fila+1}"]=p?.FechaNacimiento?.ToString("dd/MM/yyyy")??"";datos[$"F{fila+1}"]=Edad(p?.FechaNacimiento);datos[$"B{fila+2}"]=p?.Telefono??"";
        }
        Padre(personas.FirstOrDefault(x=>x.EstadoParentesco==2)?.Persona,20);
        Padre(personas.FirstOrDefault(x=>x.EstadoParentesco==1)?.Persona,25);
        var encargado=personas.FirstOrDefault(x=>x.Persona.IdResponsable==ins.IdEncargado)?.Persona;
        datos["C30"]=encargado?.NombreCompleto??"";datos["F30"]=encargado?.Dpi??"";datos["B31"]=ins.TelefonoEncargado;
        var foto=await contexto.Documentos.AsNoTracking().FirstOrDefaultAsync(x=>x.IdAlumno==alumno.IdAlumno&&x.EstadoTipoDocumento==1,ct);
        byte[]? imagen=null;
        if(foto!=null) { await using var entrada=archivos.Abrir(foto.RutaRelativa); using var memoria=new MemoryStream(); await entrada.CopyToAsync(memoria,ct);imagen=memoria.ToArray(); }
        return plantilla.Generar(datos,imagen,alumno.TieneAdecuacionCurricular);
    }
}
