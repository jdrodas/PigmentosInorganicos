using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace pigmentos.API.Models
{
    public class Familia
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("composicion")]
        [BsonElement("composicion")]
        [BsonRepresentation(BsonType.String)]
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
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (Composicion?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}