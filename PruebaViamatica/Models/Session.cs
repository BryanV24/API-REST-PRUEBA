using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaViamatica.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaCierre { get; set; }

        // FK hacia Usuario
        
        public int UsuariosIdUsuario { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
