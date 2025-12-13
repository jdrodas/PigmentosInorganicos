using Dapper;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;
using System.Data;

namespace pigmentos.API.Repositories
{
    public class PigmentoRepository(PgsqlDbContext unContexto) : IPigmentoRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;
        public async Task<List<Pigmento>> GetAllByColorIdAsync(Guid colorId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@colorId", colorId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT  " +
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica formulaQuimica, " +
                "pigmento_numero_ci numeroCi, familia_quimica_id familiaQuimicaId, color_id colorId " +
                "FROM core.v_info_pigmentos " +
                "WHERE color_id = @colorId " +
                "ORDER BY pigmento_nombre";

            var resultadoPigmentos = await conexion
                .QueryAsync<Pigmento>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPigmentos];
        }
    }
}
