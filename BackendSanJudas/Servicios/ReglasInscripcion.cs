
namespace BackendSanJudas.Servicios;
public static class ReglasInscripcion
{
    public static string NormalizarTelefono(string telefono) => new(telefono.Where(char.IsDigit).ToArray());
    public static void ValidarTelefono(string principal,string adicional)
    {
        if (NormalizarTelefono(principal)==NormalizarTelefono(adicional)) throw new BadHttpRequestException("El teléfono del encargado debe ser distinto del registrado en sus datos personales.");
    }
    public static void ValidarCupo(int ocupados,int cupo)
    {
        if (ocupados>=cupo) throw new BadHttpRequestException("El grupo ya alcanzó su cupo máximo.",409);
    }
}
