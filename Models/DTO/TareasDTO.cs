using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.DTO
{
    public class TareasDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]

        public string Nombre { get; set; }

        public bool Estado { get; set; }
        public string Descripcion { get; set; }



    }
}
