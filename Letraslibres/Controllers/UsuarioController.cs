using Letraslibres.Db;
using Letraslibres.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Letraslibres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly LetrasLibresDbContext dbContext;
        public UsuarioController(LetrasLibresDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        //Metodo GET que obtiene todos los usuarios de la DB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await dbContext.Usuarios.ToListAsync();
        }
        //Metodo POST para crear un nuevo usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(string nombre, string correo)
        {
            //Crea una nueva instancia de usuario con los datos proporcionados
            var usuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo
            };

            //Agrega el usuario a la DB y guarda los cambios
            dbContext.Usuarios.Add(usuario);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuario);
        }


        //Metodo GET que obtiene un usuario por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosDeUsuario(Guid id)
        {
            var prestamos = await dbContext.Prestamos
                .Include(p => p.Libro) //Incluye informacion del libro relacionado
                .Include(p => p.Usuario) //Incluye informacion del usuario
                .Where(p => p.UsuarioId == id) //Filtra por usuario
                .ToListAsync();

            return prestamos;
        }
        //Metodo PUT que actualiza la informacion de un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(Guid id, Usuario usuario)
        {
            //Validacion para verificar que el ID en la URL coincida con el usuario
            if (id != usuario.Id) return BadRequest();

            //Marca el usuario como modificado y guarda los cambios
            dbContext.Entry(usuario).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        //Metodo DELETE que elimmina un usuario si no tiene prestamo pendiente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            //Busca al usuario por su ID
            var usuario = await dbContext.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");
            //Validacion para comprobar si el usuario tiene prestamos pendientes
            var prestamosPendientes = await dbContext.Prestamos
                .AnyAsync(p => p.UsuarioId == id && !p.Devuelto);
            //Si tiene prestamo pendiente, no permitira eliminarlo
            if (prestamosPendientes)
                return BadRequest("El usuario debe un libro, no se puede eliminar.");

            //Elimina los prestamos registrados del usuario en la DB
            var prestamos = await dbContext.Prestamos
                .Where(p => p.UsuarioId == id)
                .ToListAsync();

            dbContext.Prestamos.RemoveRange(prestamos);

            //Elimina al usuario de la DB y guarda los cambios
            dbContext.Usuarios.Remove(usuario);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

