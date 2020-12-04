using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Models;
using TodoList.Repositorio.IRepository;

namespace TodoList.Repositorio
{
    public class UsuarioRepository : IIUsuarioRepository
    {
        private readonly AplicationDvContext _database;

        public UsuarioRepository(AplicationDvContext database)
        {
            _database = database;
        }

        public bool ExisteUsuario(string usuario)
        {
            if (_database.Usuario.Any(x => x.Username == usuario))
            {
                return true;
            }
            return false;
        }

        public Usuario GetUsuario(int UsuarioId)
        {
            return _database.Usuario.FirstOrDefault(c => c.Id == UsuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _database.Usuario.OrderBy(c => c.Username).ToList();
        }

        public bool Guardar()
        {
            return _database.SaveChanges() >= 0;
        }

        public Usuario Login(string usuario, string password)
        {
            var user = _database.Usuario.FirstOrDefault(x => x.Username == usuario);
            if (user == null)
            {
                return null;
            }


            if (!VerificaPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public Usuario Registro(Usuario usuario,string nombre ,string password)
        {
            byte[] passwordHash, passwordSalt;

            CrearPasswordHash(password, out passwordHash, out passwordSalt);

            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _database.Usuario.Add(usuario);
            Guardar();
            return usuario;



        }




        private bool VerificaPasswordHash(string password, byte[] passwordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < hashComputado.Length; i++)
                     {
                         if (hashComputado[i] != passwordHash[i]) return false;
                      }
            }
            return true;
        }





        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }







    }



   }

