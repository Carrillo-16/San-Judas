using BackendSanJudas.Interfaces;
using BackendSanJudas.Modelos.CategoriasDocumentos;
using Microsoft.AspNetCore.Mvc;
namespace BackendSanJudas.Controladores;
[Route("Api/CategoriasDocumentos")]
public class CategoriasDocumentosControlador(ICategoriasDocumentosServicio servicio) : CrudControlador<CrearCategoriasDocumentosDto,ActualizarCategoriasDocumentosDto,ConsultarCategoriasDocumentosDto>(servicio);
