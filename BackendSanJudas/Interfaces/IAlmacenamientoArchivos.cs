namespace BackendSanJudas.Interfaces;
public interface IAlmacenamientoArchivos
{
    Task<(string Ruta,string TipoContenido)> GuardarAsync(IFormFile archivo,bool soloImagen,CancellationToken cancelacion);
    Stream Abrir(string rutaRelativa);
    void Eliminar(string rutaRelativa);
}
