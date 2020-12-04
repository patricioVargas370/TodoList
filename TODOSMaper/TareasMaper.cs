using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Models.DTO;

namespace TodoList.TODOSMaper
{
    public class TareasMaper : Profile
    {
        public TareasMaper()
        {
            CreateMap<Tareas, TareasDTO>().ReverseMap();
        
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        }
    }
}
