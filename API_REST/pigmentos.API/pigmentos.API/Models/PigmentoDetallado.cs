using System.Text.Json.Serialization;

namespace pigmentos.API.Models
{
    public class PigmentoDetallado : Pigmento
    {
        [JsonPropertyName("color_nombre")]
        public string? ColorNombre { get; set; } = string.Empty;

        [JsonPropertyName("color_representacion_hexadecimal")]
        public string? ColorRepresentacionHexadecimal { get; set; } = string.Empty;

        [JsonPropertyName("familia_quimica_nombre")]
        public string? FamiliaQuimicaNombre { get; set; } = string.Empty;

        [JsonPropertyName("familia_quimica_composicion")]
        public string? FamiliaQuimicaComposicion { get; set; } = string.Empty;
    }
}
