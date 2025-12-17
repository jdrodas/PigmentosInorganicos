using Dapper;
using Npgsql;
using pigmentos.API.DbContexts;
using pigmentos.API.Exceptions;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;
using System.Data;

namespace pigmentos.API.Repositories
{
    public class PigmentoRepository(PgsqlDbContext unContexto) : IPigmentoRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Pigmento>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT  " +
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica FormulaQuimica, pigmento_numero_ci numeroCi, " +
                "familia_quimica_id familiaQuimicaId, familia_quimica_nombre familiaQuimicaNombre, familia_quimica_composicion familiaQuimicaComposicion, " +
                "color_id colorId, color_nombre colorNombre, color_representacion_hexadecimal colorRepresentacionHexadecimal " +
                "FROM core.v_info_pigmentos ";

            var resultadoPigmentos = await conexion
                .QueryAsync<Pigmento>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoPigmentos];
        }

        public async Task<Pigmento> GetByIdAsync(Guid pigmentoId)
        {
            Pigmento unPigmento = new();
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
                .QueryAsync<Pigmento>(sentenciaSQL, parametrosSentencia);

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
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica FormulaQuimica, pigmento_numero_ci numeroCi, " +
                "familia_quimica_id familiaQuimicaId, familia_quimica_nombre familiaQuimicaNombre, familia_quimica_composicion familiaQuimicaComposicion, " +
                "color_id colorId, color_nombre colorNombre, color_representacion_hexadecimal colorRepresentacionHexadecimal " +
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
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica FormulaQuimica, pigmento_numero_ci numeroCi, " +
                "familia_quimica_id familiaQuimicaId, familia_quimica_nombre familiaQuimicaNombre, familia_quimica_composicion familiaQuimicaComposicion, " +
                "color_id colorId, color_nombre colorNombre, color_representacion_hexadecimal colorRepresentacionHexadecimal " +
                "FROM core.v_info_pigmentos " +
                "WHERE familia_quimica_id = @familiaId " +
                "ORDER BY pigmento_nombre";

            var resultadoPigmentos = await conexion
                .QueryAsync<Pigmento>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoPigmentos];
        }

        public async Task<Pigmento> GetByDetailsAsync(Pigmento unPigmento)
        {
            Pigmento unPigmentoExistente = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombre", unPigmento.Nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@formula_quimica", unPigmento.FormulaQuimica,
                        DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@numero_ci", unPigmento.NumeroCi,
                        DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT  " +
                "pigmento_id id, pigmento_nombre nombre, pigmento_formula_quimica FormulaQuimica, pigmento_numero_ci numeroCi, " +
                "familia_quimica_id familiaQuimicaId, familia_quimica_nombre familiaQuimicaNombre, familia_quimica_composicion familiaQuimicaComposicion, " +
                "color_id colorId, color_nombre colorNombre, color_representacion_hexadecimal colorRepresentacionHexadecimal " +
                "FROM core.v_info_pigmentos " +
                "WHERE LOWER(pigmento_nombre) = LOWER(@nombre) " +
                "AND LOWER(pigmento_formula_quimica) = LOWER(@formula_quimica) " +
                "AND LOWER(pigmento_numero_ci) = LOWER(@numero_ci) ";

            var resultado = await conexion
                .QueryAsync<Pigmento>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unPigmentoExistente = resultado.First();

            return unPigmentoExistente;
        }

        public async Task<bool> CreateAsync(Pigmento unPigmento)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_pigmento";
                var parametros = new
                {
                    p_nombre = unPigmento.Nombre,
                    p_formula_quimica = unPigmento.FormulaQuimica,
                    p_numero_ci = unPigmento.NumeroCi,
                    p_familia_quimica_id = unPigmento.FamiliaQuimicaId,
                    p_color_id = unPigmento.ColorId
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Pigmento unPigmento)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_pigmento";
                var parametros = new
                {
                    p_id = unPigmento.Id,
                    p_nombre = unPigmento.Nombre,
                    p_formula_quimica = unPigmento.FormulaQuimica,
                    p_numero_ci = unPigmento.NumeroCi,
                    p_familia_quimica_id = unPigmento.FamiliaQuimicaId,
                    p_color_id = unPigmento.ColorId
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }
    }
}
