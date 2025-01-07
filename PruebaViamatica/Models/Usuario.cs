using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaViamatica.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [Column(TypeName ="varchar (50)")]
        public string UseName { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar (50)")]
        public string Password { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar (120)")]
        public string Mail { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar (1)")]
        public char SessionActive { get; set; }

        [Required]
        [Column(TypeName = "varchar (20)")]
        public char Status { get; set; }

        // FK hacia Persona
        public int PersonaIdPersona2 { get; set; }
        public Persona Persona { get; set; } = null!;

        // Relación con Roles (Muchos a Muchos)
        public ICollection<RolUsuario> RolesUsuarios { get; set; } = null!;

        // Relación con Sessions (Uno a Muchos)
        public ICollection<Session> Sessions { get; set; } = null!;
    }


}