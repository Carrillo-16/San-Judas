export type Registro = Record<string, string | number | boolean | null>
export interface Pagina { elementos: Registro[]; total: number; pagina:number; tamanoPagina:number }
export interface Opcion { valor: string | number; texto: string }
export interface Campo { clave:string; titulo:string; tipo?:'text'|'date'|'number'|'checkbox'|'select'; opciones?:Opcion[]; origen?:string; requerido?:boolean; max?:number; paso?:string; textoAyuda?:string }
export interface Modulo { titulo:string; descripcion:string; ruta:string; id:string; campos:Campo[]; columnas:string[] }
