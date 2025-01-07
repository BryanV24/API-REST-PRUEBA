using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaViamatica.DTOs;
using PruebaViamatica.Models;
using System;
using System.Threading.Tasks;

namespace PruebaViamatica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller
    {
        private readonly AplicationDbContext _context;

        public SessionController(AplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Validación básica de los datos del DTO
            if (string.IsNullOrEmpty(loginDto.UseName) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Usuario o contraseña no válidos.");
            }

            // Buscar al usuario en la base de datos, buscando por UseName o por CorreoElectronico
            var usuario = await _context.Usuarios
                .Include(u => u.Persona)
                .FirstOrDefaultAsync(u => u.UseName == loginDto.UseName || u.Mail == loginDto.UseName);

            if (usuario == null || usuario.Password != loginDto.Password) // Agrega hash si es necesario
            {
                return Unauthorized("Credenciales incorrectas.");
            }

            // Utilizamos una transacción para evitar sesiones duplicadas
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Verifica si el usuario ya tiene una sesión activa
                    var sesionActiva = await _context.Sessions
                        .FirstOrDefaultAsync(s => s.UsuariosIdUsuario == usuario.IdUsuario && s.FechaCierre == null);

                    if (sesionActiva != null)
                    {
                        return Conflict("El usuario ya tiene una sesión activa.");
                    }

                    // Registra la nueva sesión
                    var session = new Session
                    {
                        UsuariosIdUsuario = usuario.IdUsuario,
                        FechaIngreso = DateTime.Now
                    };

                    _context.Sessions.Add(session);
                    await _context.SaveChangesAsync(); // Guardar la sesión en la base de datos

                    // Confirma la transacción
                    await transaction.CommitAsync();

                    // Aquí puedes devolver el token JWT o la información del usuario, si es necesario
                    return Ok(new { message = "Inicio de sesión exitoso." });
                }
                catch (Exception)
                {
                    // En caso de error, revertir la transacción
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        // Cerrar sesión del usuario
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO logoutDto)
        {
            // Buscar la sesión activa
            var session = await _context.Sessions
                .FirstOrDefaultAsync(s => s.UsuariosIdUsuario == logoutDto.UsuariosIdUsuario && s.FechaCierre == null);

            if (session == null)
            {
                return BadRequest("No se encontró una sesión activa.");
            }

            // Cerrar la sesión
            session.FechaCierre = DateTime.Now;
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();

            // Llamar al procedimiento almacenado para registrar el cierre de sesión
            var parameters = new[]
            {
        new SqlParameter("@UsuariosIdUsuario", session.UsuariosIdUsuario),
        new SqlParameter("@FechaIngreso", session.FechaIngreso), // Usar la fecha de ingreso de la sesión cerrada
        new SqlParameter("@FechaCierre", session.FechaCierre) // Usar la fecha de cierre recién asignada
    };

            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_RegistrarSesion @UsuariosIdUsuario, @FechaIngreso, @FechaCierre", parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el cierre de sesión: " + ex.Message);
            }

            return Ok(new { message = "Cierre de sesión exitoso." });
        }


    }

    }

