using System.Text.Json.Serialization;

namespace Letraslibres.Models.Entities
{
    //clase Préstamo, representa un préstamo de un libro a un usuario en el sistema de Letras Libres.
    public class Prestamo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid LibroId { get; set; }

        // Se ignora durante la serialización JSON para evitar ciclos de referencia.
        [JsonIgnore]
        // Referencia al libro asociado al préstamo.
        public Libro? Libro { get; set; }

        public Guid UsuarioId { get; set; }

        // Se ignora durante la serialización JSON para evitar ciclos de referencia.
        [JsonIgnore]
        // Referencia al usuario asociado al préstamo.
        public Usuario? Usuario { get; set; } 

        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }

        // Indica si el libro ha sido devuelto o no.
        // Valor por defecto: false.
        public bool Devuelto { get; set; } = false;
    }
}
