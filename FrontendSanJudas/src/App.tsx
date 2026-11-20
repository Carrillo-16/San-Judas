import { Component } from 'react'
import { Alumnos } from './Paginas/Alumnos'
import { Responsables } from './Paginas/Responsables'
import { Inscripciones } from './Paginas/Inscripciones'
import { Grados } from './Paginas/Grados'
import { Grupos } from './Paginas/Grupos'
import { CiclosEscolares } from './Paginas/CiclosEscolares'
import { Areas } from './Paginas/Areas'
import { GradosAreas } from './Paginas/GradosAreas'
import { CategoriasDocumentos } from './Paginas/CategoriasDocumentos'
import { Documentos } from './Paginas/Documentos'
import { Calificaciones } from './Paginas/Calificaciones'
import { Api } from './Servicios/Api'
const paginas={Alumnos,Responsables,Inscripciones,Grupos,Grados,CiclosEscolares,Areas,GradosAreas,CategoriasDocumentos,Documentos,Calificaciones}
type Pagina=keyof typeof paginas
const nombres:Record<Pagina,string>={Alumnos:'Alumnos',Responsables:'Padres y responsables',Inscripciones:'Inscripciones',Grupos:'Grupos y secciones',Grados:'Grados',CiclosEscolares:'Ciclos escolares',Areas:'Áreas',GradosAreas:'Áreas por grado',CategoriasDocumentos:'Categorías',Documentos:'Actas y documentos',Calificaciones:'Calificaciones'}
export default class App extends Component<object,{pagina:Pagina;error:string}> {
 state:{pagina:Pagina;error:string}={pagina:'Alumnos',error:''}
 render(){const PaginaActual=paginas[this.state.pagina];return <div className="aplicacion"><aside><div className="marca"><img src="/Escudo.jpeg" alt="Escudo de la escuela"/><div><strong>San Judas</strong><small>E.O.R.M · Dirección</small></div></div><p className="nav-etiqueta">GESTIÓN ESCOLAR</p><nav aria-label="Gestión escolar">{(['Alumnos','Responsables','Inscripciones','Calificaciones','Documentos'] as Pagina[]).map(k=><button key={k} className={this.state.pagina===k?'activo':''} onClick={()=>this.setState({pagina:k})}>{nombres[k]}</button>)}</nav><p className="nav-etiqueta">ORGANIZACIÓN</p><nav aria-label="Catálogos">{(['CiclosEscolares','Grados','Grupos','Areas','GradosAreas','CategoriasDocumentos'] as Pagina[]).map(k=><button key={k} className={this.state.pagina===k?'activo':''} onClick={()=>this.setState({pagina:k})}>{nombres[k]}</button>)}</nav><div className="sidebar-pie"><strong>Todo en su lugar.</strong><p>Información de alumnos y documentos de la escuela.</p></div></aside><div className="principal"><div className="barra-superior"><span>ESCUELA OFICIAL RURAL MIXTA</span><button className="enlace" onClick={async()=>{try{await Api.descargar('Plantillas/Alumno/Pdf','Plantilla_Alumno.pdf')}catch(e){this.setState({error:(e as Error).message})}}}>Descargar ficha en blanco</button></div><main>{this.state.error&&<p role="alert" className="error">{this.state.error}</p>}<PaginaActual key={this.state.pagina}/></main><div className="pie-pagina">San Judas · Gestión de dirección</div></div></div>}
}
