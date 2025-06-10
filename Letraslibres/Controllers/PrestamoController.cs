using Letraslibres.Db;
using Letraslibres.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Letraslibres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        public readonly LetrasLibresDbContext dbContext;

        public PrestamoController(LetrasLibresDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //Metodo GET que obtiene los prestamos, incluyendo libros y usuarios de la DB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamo()
        {
            var prestamos = await dbContext.Prestamos
                .Include(p => p.Libro) //Incluye informacion del libro relacionado
                .Include(p => p.Usuario) //Incluye informacion del usuario relacionado
                .ToListAsync();

            return prestamos;
        }

        //Metodo POST para registrar nuevo prestamo
        [HttpPost]
        public async Task<IActionResult> RegistrarPrestamo(Guid libroId, Guid usuarioId)
        {   //Verifica si el libro y usario existen en la DB
            var libroExiste = await dbContext.Libros.AnyAsync(l => l.Id == libroId);
            var usuarioExiste = await dbContext.Usuarios.AnyAsync(u => u.Id == usuarioId);
            
            //Validacion para cuando no se encuentra el libro o usuario en la DB
            if (!libroExiste || !usuarioExiste)
                return NotFound("Libro o usuario no encontrado.");

            //Validacion si el libro se encuentra prestado
            var estaPrestado = await dbContext.Prestamos.AnyAsync(p => p.LibroId == libroId && !p.Devuelto);
            if (estaPrestado)
                return BadRequest("Este libro ya está prestado.");

            //Crea un nuevo prestamo
            var prestamo = new Prestamo
            {
                Id = Guid.NewGuid(),
                LibroId = libroId,
                UsuarioId = usuarioId,
                FechaPrestamo = DateTime.UtcNow,
                Devuelto = false
            };
            //Agrega el prestamo a la DB y guarda los cambios
            dbContext.Prestamos.Add(prestamo);
            await dbContext.SaveChangesAsync();

            //Recupera el prestamo con detalles de libro y usuario para devolver respuesta 
            var prestamoConDetalles = await dbContext.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == prestamo.Id);

            return Ok(prestamoConDetalles);
        }

        //Metodo POST para registrar la devolucion de un prestamo 
        [HttpPost("/api/devoluciones")]
        public async Task<IActionResult> RegistrarDevolucion(Guid prestamoId)
        {
            //Busca el prestamo por ID
            var prestamo = await dbContext.Prestamos.FindAsync(prestamoId);
            if (prestamo == null)
                return NotFound("Préstamo no encontrado.");

            //Verifica si ya fue devuelto anteriormente
            if (prestamo.Devuelto)
                return BadRequest("Este préstamo ya fue devuelto.");

            //Marca el prestamo como devuelto y registra la fecha de devolucion
            prestamo.Devuelto = true;
            prestamo.FechaDevolucion = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();

            return Ok("Devolución registrada correctamente.");
        }
        //Metodo PUT para actualizar un prestamo existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(Guid id, Prestamo prestamo)
        {
            //Valida que el ID coincida con el prestamo
            if (id != prestamo.Id) return BadRequest();

            //Marca el prestamo como modificado y guarda los cambios
            dbContext.Entry(prestamo).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        //Metodo DELETE para eliminar un registro de prestamo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(Guid id)
        {
            //Busca el prestamo por ID
            var prestamo = await dbContext.Prestamos.FindAsync(id);

            //Si el ID no existe, arrojara que no hay un prestamo existente
            if (prestamo == null)
                return NotFound("No hay prestamo existente.");

            //Verifica si el libro que esta asociado al prestamo aun sigue en prestamo
            var estaPrestado = await dbContext.Prestamos
                .AnyAsync(p => p.LibroId == id && !p.Devuelto);

            //Verifica que si no se ha devuelto, no permitirá la eliminación
            if (prestamo.Devuelto == false)
                return BadRequest("Libro prestado.");

            //Elimina el prestamo de la DB y guarda los cambios
            dbContext.Prestamos.Remove(prestamo);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}