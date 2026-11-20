import { Component } from 'react'
import { CrudPagina } from '../Componentes/CrudPagina'
import { modulos } from '../Modelos/Modulos'
export class Grupos extends Component { render() { return <CrudPagina modulo={modulos.Grupos}/> } }
