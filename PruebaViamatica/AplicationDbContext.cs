using Microsoft.EntityFrameworkCore;
using PruebaViamatica.Models;

namespace PruebaViamatica
{
    public class AplicationDbContext: DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
       : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolUsuario> RolesUsuarios { get; set; }
        public DbSet<RolOpcion> RolesOpciones { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configuracion del key
            modelBuilder.Entity<Persona>()
                .HasKey(p => p.IdPersona);

            modelBuilder.Entity<Session>()
                .HasKey(p => p.Id);

            // Configuración de tabla intermedia RolUsuario
            modelBuilder.Entity<RolUsuario>()
                .HasKey(ru => new { ru.RolIdRol, ru.UsuariosIdUsuario });

            modelBuilder.Entity<RolUsuario>()
                .HasOne(ru => ru.Rol)
                .WithMany(r => r.RolesUsuarios)
                .HasForeignKey(ru => ru.RolIdRol)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolUsuario>()
                .HasOne(ru => ru.Usuario)
                .WithMany(u => u.RolesUsuarios)
                .HasForeignKey(ru => ru.UsuariosIdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de tabla intermedia RolOpcion
            modelBuilder.Entity<RolOpcion>()
                .HasKey(ro => new { ro.RolIdRol, ro.RolOpcionesIdOpcion });

            modelBuilder.Entity<RolOpcion>()
                .HasOne(ro => ro.Rol)
                .WithMany(r => r.RolesOpciones)
                .HasForeignKey(ro => ro.RolIdRol)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Persona-Usuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Persona)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(u => u.PersonaIdPersona2)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario-Sessions
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Usuario)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UsuariosIdUsuario)
                .OnDelete(DeleteBehavior.Restrict);


            // Configuraciones
            modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Mail)
            .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.UseName)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-A0N0V67\\SQLEXPRESS;Database=PruebaDB;Trusted_Connection=True;TrustServerCertificate=True;");

        }
    }
}
