import type { Pagina, Registro } from '../Modelos/Tipos'
export class Api {
  static base = import.meta.env.VITE_API_URL || ''
  static async solicitud<T>(ruta:string, opciones:RequestInit={}):Promise<T> {
    let respuesta:Response
    try { respuesta=await fetch(this.base+'/Api/'+ruta,{...opciones,headers:opciones.body instanceof FormData?opciones.headers:{'Content-Type':'application/json',...opciones.headers}}) }
    catch { throw new Error('No se pudo conectar con el servidor. Verifica que el backend esté iniciado.') }
    if(!respuesta.ok) {
      const datos=await respuesta.json().catch(()=>null)
      const errores=datos?.errors?Object.values(datos.errors).flat().join(' '):''
      throw new Error(errores || datos?.title || `No se pudo completar la operación (${respuesta.status}).`)
    }
    if(respuesta.status===204)return undefined as T
    return respuesta.json() as Promise<T>
  }
  static async todos(ruta:string):Promise<Registro[]> {
    const registros:Registro[]=[];let pagina=1
    while(true) {const resultado=await this.solicitud<Pagina>(`${ruta}?Pagina=${pagina}&TamanoPagina=100`);registros.push(...resultado.elementos);if(registros.length>=resultado.total)break;pagina++}
    return registros
  }
  static async descargar(ruta:string,nombre:string) {
    const respuesta=await fetch(this.base+ (ruta.startsWith('/Api/')?ruta:'/Api/'+ruta))
    if(!respuesta.ok){const datos=await respuesta.json().catch(()=>null);throw new Error(datos?.title||'No se pudo descargar el documento.')}
    const url=URL.createObjectURL(await respuesta.blob());const enlace=document.createElement('a');enlace.href=url;enlace.download=nombre;enlace.click();setTimeout(()=>URL.revokeObjectURL(url),30000)
  }
}
