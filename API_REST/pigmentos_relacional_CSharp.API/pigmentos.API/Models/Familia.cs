using System.Text.Json.Serialization;

namespace pigmentos.API.Models
{
    public class Familia
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("composicion")]
        public string? Composicion { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraFamilia = (Familia)obj;

            return Id == otraFamilia.Id
                && Nombre!.Equals(otraFamilia.Nombre)
                && Composicion!.Equals(otraFamilia.Composicion);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Composicion?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}