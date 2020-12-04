using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Models.DTO;
using TodoList.Repositorio.IRepository;

namespace TodoList.Controllers
{
    [Authorize]
    [Route("api/Tareas")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TareasController : Controller
    {
        private readonly IITareasRepository _taRepo;
        private readonly IMapper _mapper;

        public TareasController(IITareasRepository taRepo, IMapper mapper)
        {
            _taRepo = taRepo;
            _mapper = mapper;
        }
     

        /// <summary>
        /// OBTENER TODAS LAS TAREAS
        /// </summary>
        /// <returns></returns>
       
        //[AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(201, Type = typeof(TareasDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTareas()
        {
            var listaTareas = _taRepo.GetTareas();


            var listaTareasDTO = new List<TareasDTO>();

            foreach ( var lista in listaTareas)
            {
                listaTareasDTO.Add(_mapper.Map<TareasDTO>(lista));

            }

            return Ok(listaTareasDTO);
        }
       
        
        
        //LLAMAMOS A CADA TAREA INDIVIDUALMENTE METODO GET

        /// <summary>
        /// OBTENER UNA TAREA INDIVIDUAL
        /// </summary>
        /// <param name="TareasId">ESTE ES EL ID DE LA TAREA</param>
        /// <returns></returns>
        [HttpGet("{TareasId:int}", Name = "GetTarea")]
        [ProducesResponseType(200, Type = typeof(TareasDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTarea(int TareasId)
        {
            var itemTarea = _taRepo.GetTarea(TareasId);

            if( itemTarea == null)
            {
                return NotFound();
            }

            var itemTareaDTO = _mapper.Map<TareasDTO>(itemTarea);

            return Ok(itemTareaDTO);

        }

        //AÑADIMOS TAREAS A LA LISTA DE TAREAS METODO POST

        /// <summary>
        /// CREAR UNA NUEVA TAREA
        /// </summary>
        /// <param name="tareasDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TareasDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearTarea([FromBody] TareasDTO tareasDTO)
        {
            if (tareasDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (_taRepo.ExisteTarea(tareasDTO.Nombre))
            {
                ModelState.AddModelError("", "La tarea ya existe");
                return StatusCode(404, ModelState);
            }
            var tarea = _mapper.Map<Tareas>(tareasDTO);

            if (!_taRepo.CrearTarea(tarea))
            {
                ModelState.AddModelError("", $"Algo Salio mal Guardando el registro{tarea.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTarea", new { tareasID = tarea.Id}, tarea);

        }

        //CREAMOS METODO PARA ACTUALIZAR TAREAS
        /// <summary>
        /// ACTUALIZAR UNA TAREA EXISTENTE
        /// </summary>
        /// <param name="tareasID"></param>
        /// <param name="tareasDTO"></param>
        /// <returns></returns>
        [HttpPatch("{TareasId:int}", Name = "ActualizarTarea")]
        [ProducesResponseType(204, Type = typeof(TareasDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarTarea(int tareasID, [FromBody]TareasDTO tareasDTO)
        {

            if (tareasDTO == null || tareasID != tareasDTO.Id)
            {
                return BadRequest(ModelState);

            }

            var tarea = _mapper.Map<Tareas>(tareasDTO);
            if (!_taRepo.ActualizarTarea(tarea))
            {
                ModelState.AddModelError("", $"Algo Salio mal Actualizando el registro{tarea.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //BORRAMOS UNA TAREA 

        /// <summary>
        /// BORRAMOS UNA TAREA EXISTENTE
        /// </summary>
        /// <param name="tareasID"></param>
        /// <returns></returns>
        [HttpDelete("{TareasId:int}", Name = "BorrarTarea")]       
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarTarea(int tareasID)
        {
            
            if (!_taRepo.ExisteTarea(tareasID))
            {
                return NotFound();
            }

            var tarea = _taRepo.GetTarea(tareasID);

            if (!_taRepo.BorrarTarea(tarea))
            {
                ModelState.AddModelError("", $"Algo Salio mal Borrando el registro{tarea.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}
