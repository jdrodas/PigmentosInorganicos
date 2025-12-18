using System.Text.Json.Serialization;

namespace pigmentos.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("totalColores")]
        public long Colores { get; set; } = 0;

        [JsonPropertyName("totalFamiliasQuimicas")]
        public long FamiliasQuimicas { get; set; } = 0;

        [JsonPropertyName("totalPigmentos")]
        public long Pigmentos { get; set; } = 0;
    }
}