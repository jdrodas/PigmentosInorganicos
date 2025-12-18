using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace pigmentos.API.Models
{
    public class Pigmento
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("formula_quimica")]
        [BsonElement("formula_quimica")]
        [BsonRepresentation(BsonType.String)]
        public string? FormulaQuimica { get; set; } = string.Empty;


        [JsonPropertyName("numero_ci")]
        [BsonElement("numero_ci")]
        [BsonRepresentation(BsonType.String)]
        public string? NumeroCi { get; set; } = string.Empty;

        [JsonPropertyName("color_id")]
        [BsonElement("color_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ColorId { get; set; } = string.Empty;

        [JsonPropertyName("familia_quimica_id")]
        [BsonElement("familia_quimica_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? FamiliaQuimicaId { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroPigmento = (Pigmento)obj;

            return Id == otroPigmento.Id
                && Nombre!.Equals(otroPigmento.Nombre)
                && FormulaQuimica!.Equals(otroPigmento.FormulaQuimica)
                && NumeroCi!.Equals(otroPigmento.NumeroCi)
                && ColorId!.Equals(otroPigmento.ColorId)
                && FamiliaQuimicaId!.Equals(otroPigmento.FamiliaQuimicaId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + (Id?.GetHashCode() ?? 0);
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (FormulaQuimica?.GetHashCode() ?? 0);
                hash = hash * 5 + (NumeroCi?.GetHashCode() ?? 0);
                hash = hash * 5 + (ColorId?.GetHashCode() ?? 0);
                hash = hash * 5 + (FamiliaQuimicaId?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}