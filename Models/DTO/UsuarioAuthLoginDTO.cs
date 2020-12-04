using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.DTO
{
    public class UsuarioAuthLoginDTO
    {

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "La Password es obligatoria")]
        public string Password { get; set; }
    }
}
