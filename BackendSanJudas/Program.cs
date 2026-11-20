using BackendSanJudas.Entidades;
using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos;
using BackendSanJudas.Servicios;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Fonts;

var constructor = WebApplication.CreateBuilder(args);
constructor.Services.AddDbContext<DGDbContext>(opciones => opciones.UseSqlServer(constructor.Configuration.GetConnectionString("DefaultConnection")));
constructor.Services.Configure<ArchivosOpciones>(constructor.Configuration.GetSection("Archivos"));
constructor.Services.AddScoped<IAlumnosResponsablesServicio,AlumnosResponsablesServicio>();
constructor.Services.AddScoped<IAlumnosServicio,AlumnosServicio>();
constructor.Services.AddScoped<IAreasServicio,AreasServicio>();
constructor.Services.AddScoped<IBoletasServicio,BoletasServicio>();
constructor.Services.AddScoped<ICalificacionesServicio,CalificacionesServicio>();
constructor.Services.AddScoped<ICategoriasDocumentosServicio,CategoriasDocumentosServicio>();
constructor.Services.AddScoped<ICiclosEscolaresServicio,CiclosEscolaresServicio>();
constructor.Services.AddScoped<IDocumentosServicio,DocumentosServicio>();
constructor.Services.AddScoped<IGradosAreasServicio,GradosAreasServicio>();
constructor.Services.AddScoped<IGradosServicio,GradosServicio>();
constructor.Services.AddScoped<IGruposServicio,GruposServicio>();
constructor.Services.AddScoped<IInscripcionesServicio,InscripcionesServicio>();
constructor.Services.AddScoped<IInscripcionPdfServicio,InscripcionPdfServicio>();
constructor.Services.AddScoped<IResponsablesServicio,ResponsablesServicio>();
constructor.Services.AddScoped<IAlmacenamientoArchivos,AlmacenamientoArchivos>();
constructor.Services.AddScoped<PlantillaAlumnoPdf>();
constructor.Services.AddControllers();
constructor.Services.AddExceptionHandler<ManejadorExcepciones>();
constructor.Services.AddProblemDetails();
constructor.Services.AddEndpointsApiExplorer();
constructor.Services.AddSwaggerGen();
constructor.Services.AddCors(opciones => opciones.AddDefaultPolicy(politica => politica
    .WithOrigins(constructor.Configuration.GetSection("Frontend:Origenes").Get<string[]>() ?? ["http://localhost:5173"])
    .AllowAnyHeader().AllowAnyMethod()));
if (OperatingSystem.IsWindows()) GlobalFontSettings.UseWindowsFontsUnderWindows = true;
var aplicacion = constructor.Build();
aplicacion.UseExceptionHandler();
if (aplicacion.Environment.IsDevelopment()) { aplicacion.UseSwagger(); aplicacion.UseSwaggerUI(); }
aplicacion.UseCors();
aplicacion.MapControllers();
aplicacion.Run();
