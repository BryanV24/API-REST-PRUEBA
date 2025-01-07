using System.ComponentModel.DataAnnotations;

namespace PruebaViamatica.Models
{
    public class RolOpcion
    {
        [Key]
        public int RolIdRol { get; set; }
        public Rol Rol { get; set; } = null!;

        public int RolOpcionesIdOpcion { get; set; }
        public RolOpcion RolOpcionNavigation { get; set; } = null!;
    }
}
