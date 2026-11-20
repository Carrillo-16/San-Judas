using BackendSanJudas.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BackendSanJudas.Entidades;

public class DGDbContext(DbContextOptions<DGDbContext> opciones) : DbContext(opciones)
{
    public DbSet<Alumnos> Alumnos => Set<Alumnos>();
    public DbSet<Responsables> Responsables => Set<Responsables>();
    public DbSet<AlumnosResponsables> AlumnosResponsables => Set<AlumnosResponsables>();
    public DbSet<Grados> Grados => Set<Grados>();
    public DbSet<CiclosEscolares> CiclosEscolares => Set<CiclosEscolares>();
    public DbSet<Grupos> Grupos => Set<Grupos>();
    public DbSet<Inscripciones> Inscripciones => Set<Inscripciones>();
    public DbSet<Areas> Areas => Set<Areas>();
    public DbSet<GradosAreas> GradosAreas => Set<GradosAreas>();
    public DbSet<Calificaciones> Calificaciones => Set<Calificaciones>();
    public DbSet<Documentos> Documentos => Set<Documentos>();
    public DbSet<CategoriasDocumentos> CategoriasDocumentos => Set<CategoriasDocumentos>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {

        modelo.Entity<Alumnos>().HasIndex(x => x.CodigoAlumno).IsUnique();
        modelo.Entity<Responsables>().HasIndex(x => x.Dpi).IsUnique().HasFilter("[Dpi] IS NOT NULL");
        modelo.Entity<Grados>().HasIndex(x => x.Nombre).IsUnique();
        modelo.Entity<Areas>().HasIndex(x => x.Nombre).IsUnique();
        modelo.Entity<CategoriasDocumentos>().HasIndex(x => x.Nombre).IsUnique();
        modelo.Entity<CiclosEscolares>().HasIndex(x => x.Anio).IsUnique();
        modelo.Entity<Grupos>().HasIndex(x => new { x.IdGrado, x.IdCicloEscolar, x.EstadoSeccion }).IsUnique();
        modelo.Entity<Grupos>().HasAlternateKey(x => new { x.IdGrupo, x.IdCicloEscolar });
        modelo.Entity<Inscripciones>().HasIndex(x => new { x.IdAlumno, x.IdCicloEscolar }).IsUnique();
        modelo.Entity<AlumnosResponsables>().HasIndex(x => new { x.IdAlumno, x.IdResponsable }).IsUnique();
        modelo.Entity<AlumnosResponsables>().HasAlternateKey(x => new { x.IdAlumno, x.IdResponsable });
        modelo.Entity<AlumnosResponsables>().HasIndex(x => new { x.IdAlumno, x.EstadoParentesco }).IsUnique().HasFilter("[EstadoParentesco] IN (1, 2)");
        modelo.Entity<GradosAreas>().HasIndex(x => new { x.IdGrado, x.IdArea }).IsUnique();
        modelo.Entity<GradosAreas>().HasIndex(x => new { x.IdGrado, x.Orden }).IsUnique();
        modelo.Entity<Calificaciones>().HasIndex(x => new { x.IdInscripcion, x.IdArea, x.EstadoBimestre }).IsUnique();
        modelo.Entity<Calificaciones>().Property(x => x.Nota).HasPrecision(5,2);
        modelo.Entity<Inscripciones>().Property(x => x.TelefonoEncargado).HasMaxLength(25);
        modelo.Entity<Documentos>().Property(x => x.Titulo).HasMaxLength(200);
        modelo.Entity<Documentos>().Property(x => x.NombreOriginal).HasMaxLength(255);
        modelo.Entity<Documentos>().Property(x => x.RutaRelativa).HasMaxLength(200);
        modelo.Entity<Documentos>().Property(x => x.TipoContenido).HasMaxLength(100);
        modelo.Entity<Documentos>().HasIndex(x => new { x.IdAlumno, x.EstadoTipoDocumento }).IsUnique().HasFilter("[EstadoTipoDocumento] = 1");
        modelo.Entity<Documentos>().HasIndex(x => new { x.FechaDocumento, x.IdCategoriaDocumento });

        modelo.Entity<AlumnosResponsables>().HasOne(x => x.Alumnos).WithMany().HasForeignKey(x => x.IdAlumno);
        modelo.Entity<AlumnosResponsables>().HasOne(x => x.Responsables).WithMany().HasForeignKey(x => x.IdResponsable);
        modelo.Entity<Grupos>().HasOne(x => x.Grados).WithMany().HasForeignKey(x => x.IdGrado);
        modelo.Entity<Grupos>().HasOne(x => x.CiclosEscolares).WithMany().HasForeignKey(x => x.IdCicloEscolar);
        modelo.Entity<GradosAreas>().HasOne(x => x.Grados).WithMany().HasForeignKey(x => x.IdGrado);
        modelo.Entity<GradosAreas>().HasOne(x => x.Areas).WithMany().HasForeignKey(x => x.IdArea);
        modelo.Entity<Calificaciones>().HasOne(x => x.Inscripciones).WithMany().HasForeignKey(x => x.IdInscripcion);
        modelo.Entity<Calificaciones>().HasOne(x => x.Areas).WithMany().HasForeignKey(x => x.IdArea);
        modelo.Entity<Documentos>().HasOne(x => x.Alumnos).WithMany().HasForeignKey(x => x.IdAlumno);
        modelo.Entity<Documentos>().HasOne(x => x.Responsables).WithMany().HasForeignKey(x => x.IdResponsable);
        modelo.Entity<Documentos>().HasOne(x => x.CategoriasDocumentos).WithMany().HasForeignKey(x => x.IdCategoriaDocumento);

        modelo.Entity<Inscripciones>().HasOne(x=>x.Alumnos).WithMany().HasForeignKey(x=>x.IdAlumno);
        modelo.Entity<Inscripciones>().HasOne(x=>x.Grupos).WithMany().HasForeignKey(x=>x.IdGrupo);
        modelo.Entity<Inscripciones>().HasOne(x=>x.CiclosEscolares).WithMany().HasForeignKey(x=>x.IdCicloEscolar);
        modelo.Entity<Inscripciones>().HasOne(x=>x.Responsables).WithMany().HasForeignKey(x=>x.IdEncargado);
        modelo.Entity<Inscripciones>().HasOne<Grupos>().WithMany().HasForeignKey(x=>new {x.IdGrupo,x.IdCicloEscolar}).HasPrincipalKey(x=>new {x.IdGrupo,x.IdCicloEscolar});
        modelo.Entity<Inscripciones>().HasOne<AlumnosResponsables>().WithMany().HasForeignKey(x=>new {x.IdAlumno,x.IdEncargado}).HasPrincipalKey(x=>new {x.IdAlumno,x.IdResponsable});
        foreach (var relacion in modelo.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys())) relacion.DeleteBehavior = DeleteBehavior.Restrict;

        modelo.Entity<Alumnos>().Property(x=>x.NombresAlumno).HasMaxLength(100);
        modelo.Entity<Alumnos>().Property(x=>x.ApellidosAlumno).HasMaxLength(100);
        modelo.Entity<Alumnos>().Property(x=>x.CodigoAlumno).HasMaxLength(30);
        modelo.Entity<Alumnos>().Property(x=>x.Genero).HasMaxLength(20);
        modelo.Entity<Responsables>().Property(x=>x.NombreCompleto).HasMaxLength(200);
        modelo.Entity<Responsables>().Property(x=>x.Dpi).HasMaxLength(13);
        modelo.Entity<Responsables>().Property(x=>x.Telefono).HasMaxLength(25);
        modelo.Entity<Grados>().Property(x=>x.Nombre).HasMaxLength(80);
        modelo.Entity<Areas>().Property(x=>x.Nombre).HasMaxLength(150);
        modelo.Entity<CategoriasDocumentos>().Property(x=>x.Nombre).HasMaxLength(80);
        CargarCatalogos(modelo);
    }

    private static void CargarCatalogos(ModelBuilder modelo)
    {
        modelo.Entity<Grados>().HasData(new Grados { IdGrado=1, Nombre="Kínder", EdadReferencia=5 }, new Grados { IdGrado=2, Nombre="Párvulos", EdadReferencia=6 },
            new Grados { IdGrado=3, Nombre="Primero primaria" }, new Grados { IdGrado=4, Nombre="Segundo primaria" }, new Grados { IdGrado=5, Nombre="Tercero primaria" },
            new Grados { IdGrado=6, Nombre="Cuarto primaria" }, new Grados { IdGrado=7, Nombre="Quinto primaria" }, new Grados { IdGrado=8, Nombre="Sexto primaria" });
        string[] nombres = ["Comunicación y Lenguaje L-1", "Comunicación y Lenguaje L-2 (POQOMAM)", "Comunicación y Lenguaje L-3 (INGLÉS)", "Matemáticas", "Medio natural y social", "Expresión artística", "Productividad y Desarrollo", "Educación Física", "Ciencias Sociales", "Ciencias Naturales y Tecnología", "Formación ciudadana"];
        modelo.Entity<Areas>().HasData(nombres.Select((nombre,i) => new Areas { IdArea=i+1, Nombre=nombre }));
        var asignaciones = new List<GradosAreas>();
        int id=1;
        for (int grado=3; grado<=8; grado++)
        {
            int[] areas = grado<=5 ? [1,2,3,4,5,6,7,8] : [1,2,3,4,9,10,6,11,7,8];
            for (int orden=0; orden<areas.Length; orden++) asignaciones.Add(new GradosAreas { IdGradoArea=id++, IdGrado=grado, IdArea=areas[orden], Orden=orden+1 });
        }
        modelo.Entity<GradosAreas>().HasData(asignaciones);
        modelo.Entity<CategoriasDocumentos>().HasData(new CategoriasDocumentos { IdCategoriaDocumento=1, Nombre="Actas" }, new CategoriasDocumentos { IdCategoriaDocumento=2, Nombre="Cartas" }, new CategoriasDocumentos { IdCategoriaDocumento=3, Nombre="Donativos" }, new CategoriasDocumentos { IdCategoriaDocumento=4, Nombre="Entrega de víveres" });
    }
}
