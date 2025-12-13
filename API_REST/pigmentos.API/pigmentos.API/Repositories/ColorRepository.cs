using Dapper;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;
using System.Data;

namespace pigmentos.API.Repositories
{
    public class ColorRepository(PgsqlDbContext unContexto) : IColorRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Color>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, representacion_hexadecimal representacionHexadecimal " +
                "FROM core.colores ORDER BY nombre";

            var resultadoColores = await conexion
                .QueryAsync<Color>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoColores];
        }

        public async Task<Color> GetByIdAsync(Guid colorId)
        {
            Color unColor = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@colorId", colorId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, representacion_hexadecimal representacionHexadecimal " +
                "FROM core.colores " +
                "WHERE id = @colorId";

            var resultado = await conexion
                .QueryAsync<Color>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unColor = resultado.First();

            return unColor;
        }
    }
}
