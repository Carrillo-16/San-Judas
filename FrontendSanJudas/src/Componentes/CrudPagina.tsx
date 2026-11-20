import { Component } from 'react'
import type { Modulo,Pagina,Registro } from '../Modelos/Tipos'
import { Api } from '../Servicios/Api'
import { etiqueta,modulos } from '../Modelos/Modulos'
import { Formulario } from './Formulario'
import { ExpedienteAlumno } from '../Paginas/ExpedienteAlumno'
interface Props { modulo:Modulo }
interface Estado { evidencia:File|null; filas:Registro[]; catalogos:Record<string,Registro[]>; total:number; pagina:number; cargando:boolean; guardando:boolean; error:string; mensaje:string; editar:Registro|null; expediente:Registro|null; eliminar:Registro|null }
export class CrudPagina extends Component<Props,Estado> {
 state:Estado={evidencia:null,filas:[],catalogos:{},total:0,pagina:1,cargando:true,guardando:false,error:'',mensaje:'',editar:null,expediente:null,eliminar:null}
 componentDidMount(){void this.cargar()}
 cargar=async()=>{this.setState({cargando:true,error:''});try{const m=this.props.modulo;const origenes=[...new Set(m.campos.flatMap(c=>c.origen?[c.origen]:[]))];if(origenes.includes('Grupos'))origenes.push('Grados','CiclosEscolares');const pares=await Promise.all(origenes.map(async o=>[o,await Api.todos(o)] as const));const p=await Api.solicitud<Pagina>(`${m.ruta}?Pagina=${this.state.pagina}&TamanoPagina=20`);this.setState({filas:p.elementos,total:p.total,catalogos:Object.fromEntries(pares)})}catch(e){this.setState({error:(e as Error).message})}finally{this.setState({cargando:false})}}
 nuevo=()=>{const datos:Registro={};this.props.modulo.campos.forEach(c=>datos[c.clave]=c.tipo==='checkbox'?false:c.clave==='cupoMaximo'?25:null);this.setState({editar:datos,evidencia:null,error:'',mensaje:''})}
 guardar=async(e:React.FormEvent<HTMLFormElement>)=>{
  e.preventDefault();const m=this.props.modulo;let dato=this.state.editar!;
  const evidencia=m.ruta==='Alumnos'&&dato.tieneAdecuacionCurricular?this.state.evidencia:null;
  if(evidencia&&(!evidencia.name.toLowerCase().endsWith('.pdf')||evidencia.size===0||evidencia.size>10*1024*1024)){this.setState({error:'Adjunta un PDF válido de hasta 10 MB.'});return}
  this.setState({guardando:true,error:''});let guardado=false;
  try {
   const id=dato[m.id];const respuesta=await Api.solicitud<Registro|undefined>(m.ruta+(id?'/'+id:''),{method:id?'PUT':'POST',body:JSON.stringify(dato)});
   if(!id&&respuesta)dato={...dato,...respuesta};
   guardado=true;this.setState({editar:dato});
   if(evidencia){
    const archivo=new FormData();archivo.set('Archivo',evidencia);archivo.set('IdAlumno',String(dato.idAlumno));archivo.set('EstadoTipoDocumento','4');archivo.set('Titulo','Evidencia de adecuación curricular');
    const fecha=new Intl.DateTimeFormat('en-CA',{timeZone:'America/Guatemala',year:'numeric',month:'2-digit',day:'2-digit'}).formatToParts(new Date());
    const parte=(tipo:string)=>fecha.find(p=>p.type===tipo)!.value;
    archivo.set('FechaDocumento',`${parte('year')}-${parte('month')}-${parte('day')}`);
    await Api.solicitud('Documentos',{method:'POST',body:archivo});
   }
   this.setState({editar:null,evidencia:null,mensaje:evidencia?'Alumno y evidencia PDF guardados.':'Registro guardado.'});await this.cargar();
  }catch(error){this.setState({error:(guardado?'Los datos quedaron guardados, pero no se pudo adjuntar el PDF. Puedes reintentar sin crear otro alumno. ':'')+(error as Error).message})}
  finally{this.setState({guardando:false})}
 }

 borrar=async()=>{const m=this.props.modulo;this.setState({guardando:true,error:''});try{await Api.solicitud(m.ruta+'/'+this.state.eliminar![m.id],{method:'DELETE'});this.setState({eliminar:null,mensaje:'Registro eliminado.'});await this.cargar()}catch(e){this.setState({error:(e as Error).message})}finally{this.setState({guardando:false})}}
 valor=(fila:Registro,clave:string)=>{const c=this.props.modulo.campos.find(x=>x.clave===clave);if(c?.origen){const registro=this.state.catalogos[c.origen]?.find(x=>x[modulos[c.origen!].id]===fila[clave]);return registro?etiqueta(c.origen,registro,this.state.catalogos):'No encontrado'}if(c?.opciones)return c.opciones.find(x=>x.valor===fila[clave])?.texto||'—';if(typeof fila[clave]==='boolean')return fila[clave]?'Sí':'No';return String(fila[clave]??'—')}
 descargar=async(fila:Registro)=>{try{await Api.descargar(`Inscripciones/${fila.idInscripcion}/Pdf`,`Ficha-${fila.idInscripcion}.pdf`)}catch(e){this.setState({error:(e as Error).message})}}
 render(){const m=this.props.modulo;return <section><header className="cabecera"><div><p className="sobre">ADMINISTRACIÓN ESCOLAR</p><h1>{m.titulo}</h1><p>{m.descripcion}</p></div><button onClick={this.nuevo}>+ Nuevo registro</button></header>
 {this.state.error&&<div role="alert" className="error">{this.state.error}<button className="secundario" onClick={this.cargar}>Reintentar</button></div>}{this.state.mensaje&&<p role="status" className="exito">{this.state.mensaje}</p>}
 <div className="panel"><div className="panel-titulo"><h2>Registros</h2><span>{this.state.total} en total</span></div>{this.state.cargando?<p className="vacio">Cargando información…</p>:<div className="tabla-scroll"><table><thead><tr>{m.columnas.map(c=><th key={c}>{m.campos.find(x=>x.clave===c)?.titulo||c}</th>)}<th>Acciones</th></tr></thead><tbody>{this.state.filas.map(f=><tr key={String(f[m.id])}>{m.columnas.map(c=><td key={c}>{this.valor(f,c)}</td>)}<td><div className="acciones"><button className="enlace" onClick={()=>this.setState({editar:{...f},evidencia:null,error:''})}>Editar</button>{m.ruta==='Alumnos'&&<button className="enlace" onClick={()=>this.setState({expediente:f})}>Expediente</button>}{m.ruta==='Inscripciones'&&<button className="enlace" onClick={()=>this.descargar(f)}>Descargar ficha PDF</button>}<button className="enlace peligro" onClick={()=>this.setState({eliminar:f})}>Eliminar</button></div></td></tr>)}</tbody></table>{!this.state.filas.length&&<p className="vacio">{this.state.error?'No se pudo cargar la información.':'Todavía no hay registros. Usa «Nuevo registro» para comenzar.'}</p>}</div>}<div className="paginacion"><button className="secundario" disabled={this.state.pagina===1} onClick={()=>this.setState(s=>({pagina:s.pagina-1}),this.cargar)}>Anterior</button><span>Página {this.state.pagina}</span><button className="secundario" disabled={this.state.pagina*20>=this.state.total} onClick={()=>this.setState(s=>({pagina:s.pagina+1}),this.cargar)}>Siguiente</button></div></div>
 {this.state.editar&&<div className="modal"><div role="dialog" aria-modal="true" aria-label="Formulario de registro" className="dialogo"><div className="panel-titulo"><h2>{this.state.editar[m.id]?'Editar':'Nuevo'} · {m.titulo}</h2><button className="secundario" disabled={this.state.guardando} onClick={()=>this.setState({editar:null})}>Cerrar</button></div>{this.state.error&&<p role="alert" className="error">{this.state.error}</p>}<form onSubmit={this.guardar}><Formulario campos={m.campos} datos={this.state.editar} catalogos={this.state.catalogos} cambiar={(clave,valor)=>this.setState(s=>({editar:{...s.editar,[clave]:valor},evidencia:clave==='tieneAdecuacionCurricular'&&!valor?null:s.evidencia}))}/>
 {m.ruta==='Alumnos'&&this.state.editar.tieneAdecuacionCurricular&&<div className="panel" style={{background:'#f4faf7'}}><label>Evidencia de adecuación curricular (PDF opcional)<input type="file" accept=".pdf,application/pdf" disabled={this.state.guardando} onChange={e=>{const archivo=e.target.files?.[0]??null;if(archivo&&(!archivo.name.toLowerCase().endsWith('.pdf')||archivo.size===0||archivo.size>10*1024*1024)){e.target.value='';this.setState({evidencia:null,error:'Adjunta un PDF válido de hasta 10 MB.'});return}this.setState({evidencia:archivo,error:''})}}/><small>Respaldo para la directora. Máximo 10 MB. Se guardará en el expediente al guardar el alumno.</small></label>{this.state.editar.idAlumno&&<p><small>Los PDF ya registrados se consultan en «Expediente». Desmarcar esta opción no elimina documentos anteriores.</small></p>}</div>}
 <footer><button disabled={this.state.guardando}>{this.state.guardando?'Guardando…':'Guardar registro'}</button></footer></form></div></div>}
 {this.state.eliminar&&<div className="modal"><div role="dialog" aria-modal="true" aria-label="Eliminar registro" className="dialogo pequeno"><h2>Eliminar registro</h2><p>Esta acción elimina el registro seleccionado. Los registros relacionados deben retirarse primero.</p>{this.state.error&&<p role="alert" className="error">{this.state.error}</p>}<footer><button className="secundario" onClick={()=>this.setState({eliminar:null})}>Cancelar</button><button disabled={this.state.guardando} onClick={this.borrar}>Eliminar</button></footer></div></div>}
 {this.state.expediente&&<ExpedienteAlumno alumno={this.state.expediente} cerrar={()=>{this.setState({expediente:null});void this.cargar()}}/>}</section>}
}
