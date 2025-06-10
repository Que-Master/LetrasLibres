using System.Text.Json.Serialization;

namespace Letraslibres.Models.Entities
{
    // clase Usuario, representa un usuario del sistema de Letras Libres.
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;

        // Esta propiedad se ignora al serializar a JSON para evitar ciclos de referencia.
        [JsonIgnore]
        // Colección de préstamos asociados al usuario.
        public ICollection<Prestamo>? Prestamos { get; set; }
    }
}