using Letraslibres.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Letraslibres.Db;

namespace Letraslibres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        public readonly LetrasLibresDbContext dbContext;

        public LibroController(LetrasLibresDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //Metodo GET que obtiene todos los libros de la DB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
        {
            return await dbContext.Libros.ToListAsync();
        }
        //Metodo GET que obtiene un libro por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(Guid id)
        {
            var libro = await dbContext.Libros.FindAsync(id);
            if (libro == null)
                return NotFound();

            return libro;
        }
        //Metodo POST que crea un nuevo libro
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(string titulo, string autor, string isbn)
        {
            //Crea una nueva instancia del libro con los datos proporcionados
            var libro = new Libro
            {
                Titulo = titulo,
                Autor = autor,
                ISBN = isbn
            };
            //Agrega el libro a la DB y guarda los cambios
            dbContext.Libros.Add(libro);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
        }
        //Metodo PUT para actualizar un libro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(Guid id, Libro libro)
        {
            //Validacion que verifica que el ID coincida con el del libro
            if (id != libro.Id) return BadRequest();

            //Marca el libro como modificado y guarda los cambios
            dbContext.Entry(libro).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        //Metodo DELETE que elimina un libro si no está prestado
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(Guid id)
        {
            //Busca el libro por ID 
            var libro = await dbContext.Libros.FindAsync(id);
            if (libro == null)
                return NotFound("Libro no encontrado.");

            //Validacion que verifica si el libro esta actualmente prestado
            var estaPrestado = await dbContext.Prestamos
                .AnyAsync(p => p.LibroId == id && !p.Devuelto);

            //Si se encuentra prestado no permitirá eliminarlo
            if (estaPrestado)
                return BadRequest("Libro prestado, no se puede eliminar.");
            //Elimina el libro de la DB y guarda cambios
            dbContext.Libros.Remove(libro);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
