using System.Text.Json.Serialization;

namespace PigmentosGraphQL.API.Models
{
    public class Pigmento
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("formula_quimica")]
        public string? FormulaQuimica { get; set; } = string.Empty;

        [JsonPropertyName("numero_ci")]
        public string? NumeroCi { get; set; } = string.Empty;

        [JsonPropertyName("color_id")]
        public Guid ColorId { get; set; } = Guid.Empty;

        [JsonPropertyName("color_nombre")]
        public string? ColorNombre { get; set; } = string.Empty;

        [JsonPropertyName("color_representacion_hexadecimal")]
        public string? ColorRepresentacionHexadecimal { get; set; } = string.Empty;

        [JsonPropertyName("familia_quimica_id")]
        public Guid FamiliaQuimicaId { get; set; } = Guid.Empty;

        [JsonPropertyName("familia_quimica_nombre")]
        public string? FamiliaQuimicaNombre { get; set; } = string.Empty;

        [JsonPropertyName("familia_quimica_composicion")]
        public string? FamiliaQuimicaComposicion { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroPigmento = (Pigmento)obj;

            return Id == otroPigmento.Id
                && Nombre!.Equals(otroPigmento.Nombre)
                && FormulaQuimica!.Equals(otroPigmento.FormulaQuimica)
                && NumeroCi!.Equals(otroPigmento.NumeroCi);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (FormulaQuimica?.GetHashCode() ?? 0);
                hash = hash * 5 + (NumeroCi?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}