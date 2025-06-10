using Letraslibres.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Letraslibres.Db
{
    public class LetrasLibresDbContext : DbContext
    {
        public LetrasLibresDbContext(DbContextOptions<LetrasLibresDbContext> options) : base(options) { }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>()
                .Property(l => l.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Prestamo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEWID()");

            /* Define la relación uno-a-muchos entre Libro y Prestamo
               Un libro puede tener muchos préstamos, pero un préstamo está relacionado a un solo libro*/

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            /* Define la relación uno-a-muchos entre Usuario y Prestamo
             Un usuario puede tener muchos préstamos, pero un préstamo pertenece a un solo usuario*/

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Prestamos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
