using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;


namespace TodoList.Repositorio.IRepository
{
    public interface IITareasRepository
    {
        ICollection<Tareas> GetTareas();
        Tareas GetTarea(int TareasId);
        bool ExisteTarea(string nombre);
        bool ExisteTarea(int id);
        bool CrearTarea(Tareas tareas);
        bool ActualizarTarea(Tareas tareas);
        bool BorrarTarea(Tareas tareas);
        bool Guardar();

    }
}
