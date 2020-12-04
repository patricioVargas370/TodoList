using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Models.DTO;
using TodoList.Repositorio.IRepository;

namespace TodoList.Controllers
{
    [Authorize]
    [Route("api/Usuarios")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class UsuarioController : Controller
    {
        private readonly IIUsuarioRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuarioController(IIUsuarioRepository userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
        }
        //LLAMAMOS A TODoS LoS Usuario QUE HAY, METODO GET

        /// <summary>
        /// MOSTRAMOS TODOS LOS USUARIOS EXISTENTES
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(201, Type = typeof(UsuarioDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUsuarios()
        {
            var listaUsuario = _userRepo.GetUsuarios();


            var listaUsuarioDTO = new List<UsuarioDTO>();

            foreach ( var lista in listaUsuario)
            {
                listaUsuarioDTO.Add(_mapper.Map<UsuarioDTO>(lista));

            }

            return Ok(listaUsuarioDTO);
        }
       
        
        
        //LLAMAMOS A CADA USUARIO INDIVIDUALMENTE METODO GET

        /// <summary>
        /// MOSTRAMOS UN USUARIO INDIVIDUAL
        /// </summary>
        /// <param name="UsuarioId"></param>
        /// <returns></returns>
        [HttpGet("{UsuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(200, Type = typeof(UsuarioDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetUsuario(int UsuarioId)
        {
            var itemUsuario = _userRepo.GetUsuario(UsuarioId);

            if(itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDTO = _mapper.Map<UsuarioDTO>(itemUsuario);

            return Ok(itemUsuarioDTO);

        }


        //Aqui esta creado el registro de usuarios

        /// <summary>
        /// REGISTRAMOS A UN NUEVO USUARIO
        /// </summary>
        /// <param name="usuarioAuthDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Registro")]
        [ProducesResponseType(201, Type = typeof(UsuarioAuthDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Registro(UsuarioAuthDTO usuarioAuthDto)
        {
            usuarioAuthDto.Username = usuarioAuthDto.Username.ToLower();
            if (_userRepo.ExisteUsuario(usuarioAuthDto.Username))
            {
                return BadRequest("El usuario ya existe");
            }

            var UsuarioACrear = new Usuario
            {
                Username = usuarioAuthDto.Username,
                Nombre = usuarioAuthDto.Nombre
            };

            var usuarioCreado = _userRepo.Registro(UsuarioACrear,usuarioAuthDto.Nombre, usuarioAuthDto.Password);
            return Ok(usuarioCreado);

        }






        //Aqui esta creado el registro de usuarios

        /// <summary>
        /// AUTENTICAMOS A UN USUARIO EXISTENTE PARA HACER LOGIN FUNCIONANDO CON HASH Y SALT CON JWT 
        /// </summary>
        /// <param name="usuarioAuthLoginDto"></param>
        /// <returns></returns>
        
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(201, Type = typeof(UsuarioAuthLoginDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login(UsuarioAuthLoginDTO usuarioAuthLoginDto)
        {
            var usuarioDesdeRepo = _userRepo.Login(usuarioAuthLoginDto.Username, usuarioAuthLoginDto.Password);
            if (usuarioDesdeRepo == null)
            {
                return Unauthorized();
            }


            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,  usuarioDesdeRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,  usuarioDesdeRepo.Username.ToString())
            };


            //GENERACION DE TOKEN
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDEscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDEscriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }


    }
}
