using BackendSanJudas.Modelos;

using BackendSanJudas.Interfaces;
using Microsoft.Extensions.Options;
using System.IO.Compression;
namespace BackendSanJudas.Servicios;
public class AlmacenamientoArchivos(IOptions<ArchivosOpciones> opciones) : IAlmacenamientoArchivos
{
    private readonly ArchivosOpciones configuracion=opciones.Value;
    private static readonly Dictionary<string,string> Tipos = new(StringComparer.OrdinalIgnoreCase) {
        [".pdf"]="application/pdf", [".jpg"]="image/jpeg", [".jpeg"]="image/jpeg", [".png"]="image/png",
        [".doc"]="application/msword", [".docx"]="application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".xls"]="application/vnd.ms-excel", [".xlsx"]="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };
    private string Resolver(string relativa)
    {
        if (string.IsNullOrWhiteSpace(configuracion.RutaRaiz) || !Path.IsPathFullyQualified(configuracion.RutaRaiz)) throw new BadHttpRequestException("Configure Archivos:RutaRaiz con una ruta absoluta de almacenamiento privado.",503);
        string raiz=Path.GetFullPath(configuracion.RutaRaiz);
        string ruta=Path.GetFullPath(Path.Combine(raiz,relativa));
        if (Path.IsPathRooted(relativa) || !ruta.StartsWith(raiz.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)) throw new BadHttpRequestException("Ruta de archivo no válida.");
        return ruta;
    }
    public async Task<(string Ruta,string TipoContenido)> GuardarAsync(IFormFile archivo,bool soloImagen,CancellationToken cancelacion)
    {
        string extension=Path.GetExtension(archivo.FileName).ToLowerInvariant();
        if (!Tipos.ContainsKey(extension) || (soloImagen && extension is not (".jpg" or ".jpeg" or ".png"))) throw new BadHttpRequestException("Formato no permitido. Use PDF, Word, Excel, JPG o PNG; las fotos deben ser JPG o PNG.");
        if (archivo.Length<=0 || archivo.Length>Math.Min(configuracion.TamanoMaximoBytes,10*1024*1024)) throw new BadHttpRequestException("El archivo está vacío o supera el tamaño permitido.");
        await using var origen=archivo.OpenReadStream();
        using var memoria=new MemoryStream();
        byte[] buffer=new byte[81920]; int leidos; long total=0;
        while ((leidos=await origen.ReadAsync(buffer,cancelacion))>0)
        {
            total+=leidos;
            if (total>configuracion.TamanoMaximoBytes) throw new BadHttpRequestException("El archivo supera el tamaño permitido.");
            await memoria.WriteAsync(buffer.AsMemory(0,leidos),cancelacion);
        }
        byte[] datos=memoria.ToArray();
        bool firma=extension switch {
            ".pdf" => datos.AsSpan().StartsWith("%PDF-"u8),
            ".png" => datos.AsSpan().StartsWith(new byte[] {137,80,78,71,13,10,26,10}),
            ".jpg" or ".jpeg" => datos.AsSpan().StartsWith(new byte[] {255,216,255}),
            ".doc" or ".xls" => datos.AsSpan().StartsWith(new byte[] {208,207,17,224,161,177,26,225}),
            ".docx" or ".xlsx" => ValidarOffice(datos,extension),
            _ => false };
        if (!firma) throw new BadHttpRequestException("El contenido del archivo no corresponde a su extensión.");
        string relativa=$"{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid():N}{extension}";
        string destino=Resolver(relativa); Directory.CreateDirectory(Path.GetDirectoryName(destino)!);
        try
        {
            await using var salida=new FileStream(destino,FileMode.CreateNew,FileAccess.Write,FileShare.None,81920,true);
            await salida.WriteAsync(datos,cancelacion);
        }
        catch { if (File.Exists(destino)) File.Delete(destino); throw; }
        return (relativa,Tipos[extension]);
    }
    private static bool ValidarOffice(byte[] datos,string extension)
    {
        try
        {
            using var zip=new ZipArchive(new MemoryStream(datos),ZipArchiveMode.Read);
            return zip.Entries.Count<=10000 && zip.GetEntry("[Content_Types].xml")!=null && zip.GetEntry(extension==".docx" ? "word/document.xml" : "xl/workbook.xml")!=null;
        }
        catch (InvalidDataException) { return false; }
    }
    public Stream Abrir(string relativa)
    {
        string ruta=Resolver(relativa);
        if (!File.Exists(ruta)) throw new BadHttpRequestException("El archivo no está disponible en el almacenamiento.",404);
        return new FileStream(ruta,FileMode.Open,FileAccess.Read,FileShare.Read,81920,true);
    }
    public void Eliminar(string relativa) { string ruta=Resolver(relativa); if (File.Exists(ruta)) File.Delete(ruta); }
}
