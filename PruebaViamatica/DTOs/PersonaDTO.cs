namespace PruebaViamatica.DTOs
{
    public class PersonaDto
    {
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string UseName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RolId { get; set; }  // Agregar el RolId
    }

}
