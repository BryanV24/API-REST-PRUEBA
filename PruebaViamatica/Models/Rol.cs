using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaViamatica.Models
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Required]
        [Column(TypeName = "varchar (50)")]
        public string RolName { get; set; } = null!;

        // Relación con Usuarios (Muchos a Muchos)
        public ICollection<RolUsuario> RolesUsuarios { get; set; } = null!;

        // Relación con Opciones (Muchos a Muchos)
        public ICollection<RolOpcion> RolesOpciones { get; set; } = null!;
    }
}
