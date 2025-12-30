using System.Text.Json.Serialization;

namespace PigmentosGraphQL.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("totalColores")]
        public long TotalColores { get; set; } = 0;

        [JsonPropertyName("totalFamiliasQuimicas")]
        public long TotalFamiliasQuimicas { get; set; } = 0;

        [JsonPropertyName("totalPigmentos")]
        public long TotalPigmentos { get; set; } = 0;
    }
}