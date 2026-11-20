namespace BackendSanJudas.Servicios;
public static class RelojEscolar
{
    private static readonly TimeZoneInfo Zona=TimeZoneInfo.FindSystemTimeZoneById(OperatingSystem.IsWindows()?"Central America Standard Time":"America/Guatemala");
    public static DateTime Ahora => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,Zona);
    public static DateOnly Hoy => DateOnly.FromDateTime(Ahora);
}
