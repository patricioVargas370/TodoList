using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.DTO
{
    public class UsuarioAuthDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage ="El usuario es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "La Password es obligatoria")]
        [StringLength(10, MinimumLength = 4,ErrorMessage ="La contraseña debe estar entre 4 y 10 caracteres")]

        public string Password { get; set; }
    }
}
