using Microsoft.EntityFrameworkCore;
using EscuelaCrud.Models;

namespace EscuelaCrud.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Estudiante> Estudiantes { get; set; }
    }
}
