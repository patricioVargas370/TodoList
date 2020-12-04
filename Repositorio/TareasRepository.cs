using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Models;
using TodoList.Repositorio.IRepository;

namespace TodoList.Repositorio
{
    public class TareasRepository : IITareasRepository
    {
        private readonly AplicationDvContext _database;

        public TareasRepository(AplicationDvContext database)
        {
            _database = database;
        }
        public bool ActualizarTarea(Tareas tareas)
        {
            _database.Tareas.Update(tareas);
            return Guardar();
        }
        public bool BorrarTarea(Tareas tareas)
        {
            _database.Tareas.Remove(tareas);
            return Guardar();
        }

        public bool CrearTarea(Tareas tareas)
        {
            _database.Tareas.Add(tareas);
            return Guardar();
        }

        public bool ExisteTarea(string nombre)
        {
            bool valor = _database.Tareas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExisteTarea(int id)
        {
            return _database.Tareas.Any(c => c.Id == id);
        }

        public ICollection<Tareas> GetTareas()
        {
            return _database.Tareas.OrderBy(c => c.Nombre).ToList();
        }

        public Tareas GetTarea(int TareasId)
        {
            return _database.Tareas.FirstOrDefault(c => c.Id == TareasId);
        }

        public bool Guardar()
        {
            return _database.SaveChanges() >= 0;
        }
    }
}
