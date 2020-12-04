using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;

namespace TodoList.Data
{
    public class AplicationDvContext : DbContext
    {

        public AplicationDvContext(DbContextOptions<AplicationDvContext> options):base(options)
        {
        }

        public DbSet<Tareas> Tareas { get; set; }

        public DbSet<Usuario> Usuario { get; set; }


    }
}
