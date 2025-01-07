using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaViamatica.Models
{
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Required]
        [Column(TypeName = "varchar (60)")]
        public string Nombres { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar (60)")]
        public string Apellidos { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar (10)")]
        public string Identificacion { get; set; } = null!;


        public DateTime FechaNacimiento { get; set; }

        // Relación con Usuario (Uno a Uno)
        public ICollection<Usuario> Usuarios { get; set; } = null!;
    }
}
