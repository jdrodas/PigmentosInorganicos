using System.Text.Json.Serialization;

namespace pigmentos.API.Models
{
    public class Color
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("representacion_hexadecimal")]
        public string? RepresentacionHexadecimal { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroColor = (Color)obj;

            return Id == otroColor.Id
                && Nombre!.Equals(otroColor.Nombre)
                && RepresentacionHexadecimal!.Equals(otroColor.RepresentacionHexadecimal);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (RepresentacionHexadecimal?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}