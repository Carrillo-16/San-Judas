import { Component } from 'react'
import type { Campo, Registro } from '../Modelos/Tipos'
import { etiqueta,modulos } from '../Modelos/Modulos'
interface Props { campos:Campo[]; datos:Registro; catalogos:Record<string,Registro[]>; cambiar:(clave:string,valor:Registro[string])=>void }
export class Formulario extends Component<Props> {
 render(){return <div className="campos">{this.props.campos.map(c=>{
 const valor=this.props.datos[c.clave];const opciones=c.origen?(this.props.catalogos[c.origen]||[]).map(r=>({valor:String(r[modulos[c.origen!].id]),texto:etiqueta(c.origen!,r,this.props.catalogos)})):c.opciones||[]
 return <label key={c.clave} className={c.tipo==='checkbox'?'campo-check':''}><span>{c.titulo}{c.requerido?' *':''}</span>
 {c.tipo==='checkbox'?<input type="checkbox" checked={!!valor} onChange={e=>this.props.cambiar(c.clave,e.target.checked)}/>:c.tipo==='select'?<select required={c.requerido} value={String(valor??'')} onChange={e=>this.props.cambiar(c.clave,e.target.value===''?null:(c.origen||typeof c.opciones?.[0]?.valor==='number')?Number(e.target.value):e.target.value)}><option value="">Seleccionar</option>{opciones.map(o=><option key={o.valor} value={o.valor}>{o.texto}</option>)}</select>:<input type={c.tipo||'text'} required={c.requerido} maxLength={c.max} step={c.paso} value={String(valor??'')} onChange={e=>this.props.cambiar(c.clave,e.target.value===''?null:c.tipo==='number'?Number(e.target.value):e.target.value)}/>}
 {c.textoAyuda&&<small>{c.textoAyuda}</small>}</label>})}</div>}
}
