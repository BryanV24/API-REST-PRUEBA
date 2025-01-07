using System.ComponentModel.DataAnnotations;

namespace PruebaViamatica.Models
{
    public class RolUsuario
    {
        [Key]
        public int RolIdRol { get; set; }
        public Rol Rol { get; set; } = null!;

        public int UsuariosIdUsuario { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
