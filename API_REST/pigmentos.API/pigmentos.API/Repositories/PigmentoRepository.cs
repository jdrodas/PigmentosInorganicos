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

        public async Task<List<PigmentoDetallado>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT  " +
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica FormulaQuimica, pigmento_numero_ci numeroCi, " +
                "familia_quimica_id familiaQuimicaId, familia_quimica_nombre familiaQuimicaNombre, familia_quimica_composicion familiaQuimicaComposicion, " +
                "color_id colorId, color_nombre colorNombre, color_representacion_hexadecimal colorRepresentacionHexadecimal " +
                "FROM core.v_info_pigmentos ";

            var resultadoPigmentos = await conexion
                .QueryAsync<PigmentoDetallado>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoPigmentos];
        }

        public async Task<PigmentoDetallado> GetByIdAsync(Guid pigmentoId)
        {
            PigmentoDetallado unPigmento = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@pigmentoId", pigmentoId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT  " +
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica FormulaQuimica, pigmento_numero_ci numeroCi, " +
                "familia_quimica_id familiaQuimicaId, familia_quimica_nombre familiaQuimicaNombre, familia_quimica_composicion familiaQuimicaComposicion, " +
                "color_id colorId, color_nombre colorNombre, color_representacion_hexadecimal colorRepresentacionHexadecimal " +
                "FROM core.v_info_pigmentos " +
                "WHERE pigmento_id = @pigmentoId ";

            var resultado = await conexion
                .QueryAsync<PigmentoDetallado>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unPigmento = resultado.First();

            return unPigmento;
        }
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

        public async Task<List<Pigmento>> GetAllByFamilyIdAsync(Guid familiaId)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@familiaId", familiaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT  " +
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica formulaQuimica, " +
                "pigmento_numero_ci numeroCi, familia_quimica_id familiaQuimicaId, color_id colorId " +
                "FROM core.v_info_pigmentos " +
                "WHERE familia_quimica_id = @familiaId " +
                "ORDER BY pigmento_nombre";

            var resultadoPigmentos = await conexion
                .QueryAsync<Pigmento>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPigmentos];
        }

    }
}
