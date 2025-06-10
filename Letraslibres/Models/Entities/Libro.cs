using System.Text.Json.Serialization;

namespace Letraslibres.Models.Entities
{
    // clase Libro, representa un libro en el sistema de Letras Libres.
    public class Libro
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string ISBN { get; set; } = null!;

        // Esta propiedad se ignora al serializar a JSON para evitar ciclos de referencia.
        [JsonIgnore]
        // Lista de préstamos asociados al libro.
        public List<Prestamo>? Prestamos { get; set; }
    }
}
