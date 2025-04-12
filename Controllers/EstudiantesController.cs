using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EscuelaCrud.Data;
using EscuelaCrud.Models;
using System;

namespace EscuelaCrud.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        // Esto es para acceder a la base de datos de la aplicación
        private readonly AppDbContext _context;

        // Aquí se crea el constructor que se encarga de recibir el contexto de la base de datos
        public EstudiantesController(AppDbContext context)
        {
            _context = context;
        }

        // Método para obtener todos los estudiantes desde la base de datos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Usamos 'ToListAsync()' para traer todos los estudiantes de la base de datos
                var estudiantes = await _context.Estudiantes.ToListAsync();
                Console.WriteLine("GET request received");
                return Ok(estudiantes); // Devolvemos los estudiantes en formato JSON
            }
            catch (Exception ex)
            {
                // Si pasa un error, lo mostramos en la consola
                Console.WriteLine($"Error GET: {ex.Message}");
                return StatusCode(500, "Internal server error"); // Devolvemos un error 500
            }
        }

        // Método para agregar un estudiante nuevo a la base de datos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Estudiante estudiante)
        {
            Console.WriteLine("Entra");
            try
            {
                // Si el estudiante no llega bien, devolvemos un mensaje de error
                Console.WriteLine($"POST recibido: {estudiante.Cedula}, {estudiante.Nombres}, {estudiante.Apellidos}");
                if (estudiante == null)
                {
                    Console.WriteLine("Estudiante no recibido.");
                    return BadRequest("Estudiante no recibido.");
                }

                // Añadimos el estudiante a la base de datos
                _context.Estudiantes.Add(estudiante);
                await _context.SaveChangesAsync(); // Guardamos los cambios
                Console.WriteLine("Estudiante guardado exitosamente.");
                return Ok(estudiante); // Devolvemos el estudiante que fue guardado
            }
            catch (Exception ex)
            {
                // Si algo sale mal, mostramos el error en la consola
                Console.WriteLine($"Error en POST: {ex.Message}");
                return StatusCode(500, "Error al guardar el estudiante."); // Devolvemos error 500
            }
        }

        // Método para actualizar los datos de un estudiante ya existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Estudiante estudiante)
        {
            try
            {
                // Comprobamos si el ID recibido en la URL coincide con el ID del estudiante
                if (id != estudiante.Id)
                {
                    Console.WriteLine("ID no coincide.");
                    return BadRequest("ID no coincide.");
                }

                // Si todo está bien, modificamos el estado del estudiante
                _context.Entry(estudiante).State = EntityState.Modified;
                await _context.SaveChangesAsync(); // Guardamos los cambios
                Console.WriteLine("Estudiante actualizado exitosamente.");
                return Ok(estudiante); // Devolvemos el estudiante actualizado
            }
            catch (Exception ex)
            {
                // Si hay un error, lo mostramos en la consola
                Console.WriteLine($"Error en PUT: {ex.Message}");
                return StatusCode(500, "Error al actualizar el estudiante."); // Devolvemos error 500
            }
        }

        // Método para eliminar un estudiante
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Buscamos al estudiante por su ID
                var estudiante = await _context.Estudiantes.FindAsync(id);
                if (estudiante == null)
                {
                    Console.WriteLine("Estudiante no encontrado.");
                    return NotFound(); // Si no lo encontramos, devolvemos un error 404
                }

                // Si lo encontramos, lo eliminamos
                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync(); // Guardamos los cambios
                Console.WriteLine("Estudiante eliminado exitosamente.");
                return Ok(); // Devolvemos una respuesta de éxito
            }
            catch (Exception ex)
            {
                // Si algo sale mal, mostramos el error en la consola
                Console.WriteLine($"Error en DELETE: {ex.Message}");
                return StatusCode(500, "Error al eliminar el estudiante."); // Devolvemos error 500
            }
        }
    }
}
