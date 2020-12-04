using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.DTO
{
    public class UsuarioDTO
    {

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }


    }
}
