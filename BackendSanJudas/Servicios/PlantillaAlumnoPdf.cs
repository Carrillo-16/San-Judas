using System.IO.Compression;
using System.Xml.Linq;
using System.Globalization;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
namespace BackendSanJudas.Servicios;

// Lee la plantilla original en cada descarga. No modifica el Excel del usuario.
// Motor limitado al formato de esta ficha: celdas, combinaciones, texto, bordes e imágenes.
public class PlantillaAlumnoPdf(IWebHostEnvironment entorno)
{
    private static readonly XNamespace S="http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private static double Numero(XAttribute? a,double valor=0) => double.TryParse(a?.Value,NumberStyles.Any,CultureInfo.InvariantCulture,out var n)?n:valor;
    private static (int Col,int Fila) Posicion(string celda)
    {
        int col=0; foreach(char c in celda.Where(char.IsLetter)) col=col*26+c-'A'+1;
        return (col,int.Parse(new string(celda.Where(char.IsDigit).ToArray())));
    }
    public byte[] Generar(IReadOnlyDictionary<string,string> datos,byte[]? foto,bool? adecuacion)
    {
        string ruta=Path.Combine(entorno.ContentRootPath,"Plantillas","Plantilla_Alumno.xlsx");
        if(!File.Exists(ruta))throw new BadHttpRequestException("No se encontró Plantillas/Plantilla_Alumno.xlsx.",503);
        using var zip=ZipFile.OpenRead(ruta);
        XDocument Leer(string nombre) { using var flujo=zip.GetEntry(nombre)!.Open();return XDocument.Load(flujo); }
        var hoja=Leer("xl/worksheets/sheet1.xml");var estilos=Leer("xl/styles.xml");
        var textos=Leer("xl/sharedStrings.xml").Root!.Elements(S+"si").Select(si=>string.Concat(si.Descendants(S+"t").Select(t=>t.Value))).ToArray();
        var fuentes=estilos.Root!.Element(S+"fonts")!.Elements().ToArray();
        var formatos=estilos.Root.Element(S+"cellXfs")!.Elements().ToArray();
        var bordes=estilos.Root.Element(S+"borders")!.Elements().ToArray();
        double[] anchos=new double[8],altos=new double[40],x=new double[9],y=new double[41];
        for(int c=1;c<=7;c++) {
            var columna=hoja.Descendants(S+"col").FirstOrDefault(e=>Numero(e.Attribute("min"))<=c&&Numero(e.Attribute("max"))>=c);
            anchos[c]=(Numero(columna?.Attribute("width"),10.90625)*7+5)*.75;
            x[c+1]=x[c]+anchos[c];
        }
        for(int f=1;f<=39;f++) { var fila=hoja.Descendants(S+"row").FirstOrDefault(e=>Numero(e.Attribute("r"))==f);altos[f]=Numero(fila?.Attribute("ht"),15);y[f+1]=y[f]+altos[f]; }
        var combinaciones=hoja.Descendants(S+"mergeCell").Select(e=>e.Attribute("ref")!.Value).ToList();
        // Campos que en Excel no estaban combinados pero necesitan espacio para los datos.
        foreach(string rango in new[]{"C5:E5","C6:E6","C12:E12","C20:D20","C21:D21","C26:D26","C30:D30","B22:D22","B27:D27","B31:D31","F21:G21","F26:G26"})combinaciones.Add(rango);
        XRect Rectangulo(string direccion) {
            string rango=combinaciones.FirstOrDefault(v=>v.Split(':')[0]==direccion)??direccion;
            var inicio=Posicion(rango.Split(':')[0]);var fin=Posicion(rango.Split(':').Last());
            return new XRect(x[inicio.Col],y[inicio.Fila],x[fin.Col+1]-x[inicio.Col],y[fin.Fila+1]-y[inicio.Fila]);
        }
        using var pdf=new PdfDocument();pdf.Info.Title="Ficha de inscripción - E.O.R.M San Judas";
        var pagina=pdf.AddPage();pagina.Size=PdfSharp.PageSize.Letter;
        using var g=XGraphics.FromPdfPage(pagina);
        double escala=Math.Min(1,(pagina.Width.Point-100)/x[8]);
        g.TranslateTransform((pagina.Width.Point-x[8]*escala)/2,76);g.ScaleTransform(escala);
        var celdas=hoja.Descendants(S+"c").ToDictionary(c=>c.Attribute("r")!.Value);
        foreach(string direccion in celdas.Keys.Union(datos.Keys)) {
            var pos=Posicion(direccion);if(pos.Col>7||pos.Fila>39)continue;
            if(combinaciones.Any(v=>{var a=Posicion(v.Split(':')[0]);var b=Posicion(v.Split(':').Last());return direccion!=v.Split(':')[0]&&pos.Col>=a.Col&&pos.Col<=b.Col&&pos.Fila>=a.Fila&&pos.Fila<=b.Fila;}))continue;
            celdas.TryGetValue(direccion,out var celda);
            var estilo=formatos[(int)Numero(celda?.Attribute("s"),4)];
            var fuente=fuentes[(int)Numero(estilo.Attribute("fontId"))];
            var rect=Rectangulo(direccion);
            var borde=bordes[(int)Numero(estilo.Attribute("borderId"))];
            if(borde.Element(S+"bottom")?.Attribute("style")!=null)g.DrawLine(XPens.Black,rect.Left,rect.Bottom,rect.Right,rect.Bottom);
            if(borde.Element(S+"top")?.Attribute("style")!=null)g.DrawLine(XPens.Black,rect.Left,rect.Top,rect.Right,rect.Top);
            if(borde.Element(S+"left")?.Attribute("style")!=null)g.DrawLine(XPens.Black,rect.Left,rect.Top,rect.Left,rect.Bottom);
            if(borde.Element(S+"right")?.Attribute("style")!=null)g.DrawLine(XPens.Black,rect.Right,rect.Top,rect.Right,rect.Bottom);
            string valor=datos.TryGetValue(direccion,out var dato)?dato:celda?.Attribute("t")?.Value=="s"?textos[int.Parse(celda.Element(S+"v")!.Value)]:celda?.Element(S+"v")?.Value??"";
            if(direccion=="F10")valor="";
            if(string.IsNullOrEmpty(valor))continue;
            double tamano=Numero(fuente.Element(S+"sz")?.Attribute("val"),11);
            var estiloFuente=fuente.Element(S+"b")!=null?XFontStyleEx.Bold:XFontStyleEx.Regular;
            string nombre=fuente.Element(S+"name")?.Attribute("val")?.Value??"Times New Roman";
            var font=new XFont(nombre,tamano,estiloFuente);
            bool esDato=datos.ContainsKey(direccion);
            if(esDato) {
                while(g.MeasureString(valor,font).Width>rect.Width-4&&tamano>7) { tamano-=.25;font=new XFont(nombre,tamano,XFontStyleEx.Regular); }
                if(g.MeasureString(valor,font).Width>rect.Width-4) throw new BadHttpRequestException("El dato de "+direccion+" es demasiado largo para la plantilla. Amplíe ese campo en Excel.");
            }
            string? alinear=estilo.Element(S+"alignment")?.Attribute("horizontal")?.Value;
            var formato=new XStringFormat { Alignment=esDato?XStringAlignment.Near:alinear=="center"?XStringAlignment.Center:alinear=="right"?XStringAlignment.Far:XStringAlignment.Near,LineAlignment=XLineAlignment.Center };
            g.DrawString(valor,font,XBrushes.Black,new XRect(rect.X+2,rect.Y,rect.Width-4,rect.Height),formato);
        }
        XNamespace D="http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing", A="http://schemas.openxmlformats.org/drawingml/2006/main",R="http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        var dibujo=Leer("xl/drawings/drawing1.xml");
        var relaciones=Leer("xl/drawings/_rels/drawing1.xml.rels").Root!.Elements().ToDictionary(e=>e.Attribute("Id")!.Value,e=>e.Attribute("Target")!.Value);
        int casilla=0;
        foreach(var anchor in dibujo.Root!.Elements()) {
            var desde=anchor.Element(D+"from");var hasta=anchor.Element(D+"to");if(desde==null||hasta==null)continue;
            double Punto(XElement punto,bool horizontal) { int indice=int.Parse(punto.Element(D+(horizontal?"col":"row"))!.Value)+1; double offset=double.Parse(punto.Element(D+(horizontal?"colOff":"rowOff"))!.Value,CultureInfo.InvariantCulture)/12700;return (horizontal?x[indice]:y[indice])+offset; }
            var rect=new XRect(Punto(desde,true),Punto(desde,false),Punto(hasta,true)-Punto(desde,true),Punto(hasta,false)-Punto(desde,false));
            var imagen=anchor.Descendants(A+"blip").FirstOrDefault();
            if(imagen!=null) {
                string objetivo=relaciones[imagen.Attribute(R+"embed")!.Value];string nombre="xl/"+objetivo.Replace("../","");
                using var entrada=zip.GetEntry(nombre)!.Open();using var memoria=new MemoryStream();entrada.CopyTo(memoria);memoria.Position=0;
                using var logo=XImage.FromStream(memoria);g.DrawImage(logo,rect);
            } else {
                g.DrawRectangle(XPens.Black,rect);
                if(adecuacion.HasValue&&((casilla==0&&adecuacion.Value)||(casilla==1&&!adecuacion.Value)))g.DrawString("X",new XFont("Times New Roman",11),XBrushes.Black,rect,XStringFormats.Center);
                casilla++;
            }
        }
        var recuadro=Rectangulo("F10");g.DrawRectangle(XPens.Black,recuadro);
        if(foto!=null) {
            using var memoria=new MemoryStream(foto);using var imagen=XImage.FromStream(memoria);
            double factor=Math.Min((recuadro.Width-4)/imagen.PixelWidth,(recuadro.Height-4)/imagen.PixelHeight);
            double ancho=imagen.PixelWidth*factor,alto=imagen.PixelHeight*factor;
            g.DrawImage(imagen,recuadro.X+(recuadro.Width-ancho)/2,recuadro.Y+(recuadro.Height-alto)/2,ancho,alto);
        }
        using var salida=new MemoryStream();pdf.Save(salida,false);return salida.ToArray();
    }
}
