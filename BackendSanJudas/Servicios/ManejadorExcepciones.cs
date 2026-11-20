using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BackendSanJudas.Servicios;
public class ManejadorExcepciones(ILogger<ManejadorExcepciones> registro) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext contexto, Exception error, CancellationToken cancelacion)
    {
        int estado = error is BadHttpRequestException solicitud ? solicitud.StatusCode : error is DbUpdateException ? 409 : error is SqlException ? 503 : 500;
        string detalle = error is BadHttpRequestException ? error.Message : error is DbUpdateException ? "No se pudo guardar. Compruebe duplicados o registros relacionados." : error is SqlException ? "No se pudo consultar SQL Server. Verifique que la instancia esté disponible y que el script de la base esté aplicado." : "Ocurrió un error interno al procesar la solicitud.";
        if (estado == 500) registro.LogError(error, "Error interno. Solicitud {Identificador}", contexto.TraceIdentifier);
        contexto.Response.StatusCode=estado;
        await contexto.Response.WriteAsJsonAsync(new ProblemDetails { Status=estado, Title=detalle, Extensions = { ["IdentificadorSolicitud"] = contexto.TraceIdentifier } }, cancelacion);
        return true;
    }
}
