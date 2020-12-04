using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;


namespace TodoList.Repositorio.IRepository
{
    public interface IIUsuarioRepository
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int UsuarioId);
        bool ExisteUsuario(string usuario);
        Usuario Registro(Usuario usuario,string nombre ,string password);
        Usuario Login(string usuario, string password);
        bool Guardar();

    }
}
