using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaViamatica.DTOs;
using PruebaViamatica.Models;

namespace PruebaViamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly AplicationDbContext _context;

        public PersonaController(AplicationDbContext context)
        {
            _context = context;
        }

        // Crear Persona y Usuario
        [HttpPost]
        public async Task<IActionResult> CreatePersona([FromBody] PersonaDto personaDto)
        {
            // Validación de Nombre de Usuario
            if (!IsValidUserName(personaDto.UseName))
            {
                return BadRequest("El nombre de usuario no cumple con los requisitos.");
            }

            // Validación de Contraseña
            if (!IsValidPassword(personaDto.Password))
            {
                return BadRequest("La contraseña no cumple con los requisitos.");
            }

            // Validación de Identificación
            if (!IsValidIdentification(personaDto.Identificacion))
            {
                return BadRequest("La identificación no cumple con los requisitos.");
            }

            // Crear una nueva Persona
            var persona = new Persona
            {
                Nombres = personaDto.Nombres,
                Apellidos = personaDto.Apellidos,
                Identificacion = personaDto.Identificacion,
                FechaNacimiento = personaDto.FechaNacimiento
            };

            // Generar correo a partir del nombre y apellido
            string email = GenerateEmail(persona.Nombres, persona.Apellidos);

            // Comprobar que el correo no esté duplicado
            var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Mail == email);
            if (existingUser != null)
            {
                return BadRequest("El correo generado ya está en uso.");
            }

            // Crear un nuevo Usuario y asociarlo con la Persona
            var usuario = new Usuario
            {
                UseName = personaDto.UseName,
                Password = HashPassword(personaDto.Password), // Asegúrate de aplicar un hash a la contraseña
                Mail = email,
                SessionActive = 'N', // Asume que 'N' es para inactivo
                Status = 'A', // Asume que 'A' es activo
                Persona = persona
            };

            // Añadir Persona y Usuario al contexto
            _context.Personas.Add(persona);
            _context.Usuarios.Add(usuario);

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { Persona = persona, Usuario = usuario });
        }

        private bool IsValidUserName(string userName)
        {
            // Verificar longitud
            if (userName.Length < 8 || userName.Length > 20)
                return false;

            // Verificar que contenga al menos un número
            if (!userName.Any(char.IsDigit))
                return false;

            // Verificar que contenga al menos una letra mayúscula
            if (!userName.Any(char.IsUpper))
                return false;

            // Verificar que no contenga signos
            if (userName.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            // Verificar que no esté duplicado
            var existingUser = _context.Usuarios.Any(u => u.UseName == userName);
            if (existingUser)
                return false;

            return true;
        }

        private bool IsValidPassword(string password)
        {
            // Verificar longitud mínima
            if (password.Length < 8)
                return false;

            // Verificar que contenga al menos una letra mayúscula
            if (!password.Any(char.IsUpper))
                return false;

            // Verificar que no contenga espacios
            if (password.Contains(" "))
                return false;

            // Verificar que contenga al menos un signo
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }

        private bool IsValidIdentification(string identification)
        {
            // Verificar que tenga 10 dígitos
            if (identification.Length != 10 || !identification.All(char.IsDigit))
                return false;

            // Verificar que no tenga 4 dígitos consecutivos iguales
            for (int i = 0; i < identification.Length - 3; i++)
            {
                if (identification[i] == identification[i + 1] &&
                    identification[i] == identification[i + 2] &&
                    identification[i] == identification[i + 3])
                {
                    return false;
                }
            }

            return true;
        }

        private string GenerateEmail(string nombres, string apellidos)
        {
            // Generar el correo a partir del nombre y apellido
            string baseEmail = $"{nombres.Substring(0, 1).ToLower()}{apellidos.ToLower()}@mail.com";

            // Verificar si el correo ya existe
            var emailExists = _context.Usuarios.Any(u => u.Mail == baseEmail);
            if (!emailExists)
                return baseEmail;

            // Si el correo ya existe, agregar un número al final
            int counter = 1;
            string emailWithNumber = $"{nombres.Substring(0, 1).ToLower()}{apellidos.ToLower()}{counter}@mail.com";
            while (_context.Usuarios.Any(u => u.Mail == emailWithNumber))
            {
                counter++;
                emailWithNumber = $"{nombres.Substring(0, 1).ToLower()}{apellidos.ToLower()}{counter}@mail.com";
            }

            return emailWithNumber;
        }

        private string HashPassword(string password)
        {
            // Lógica para encriptar la contraseña (por ejemplo, usando bcrypt o SHA256)
            return password; // Solo un ejemplo, reemplaza con un hash real
        }
    }

}
