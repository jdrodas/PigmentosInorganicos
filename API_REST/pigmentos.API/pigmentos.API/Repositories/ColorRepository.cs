using Dapper;
using Npgsql;
using pigmentos.API.DbContexts;
using pigmentos.API.Exceptions;
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

        public async Task<Color> GetByDetailsAsync(Color unColor)
        {
            Color colorExistente = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@colorNombre", unColor.Nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@colorRepresentacionHexadecimal", unColor.RepresentacionHexadecimal,
                        DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, representacion_hexadecimal representacionHexadecimal " +
                "FROM core.colores " +
                "WHERE nombre = @colorNombre " +
                "AND representacion_hexadecimal = @colorRepresentacionHexadecimal";


            var resultado = await conexion
                .QueryAsync<Color>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                colorExistente = resultado.First();

            return colorExistente;
        }

        public async Task<bool> CreateAsync(Color unColor)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_color";
                var parametros = new
                {
                    p_nombre = unColor.Nombre,
                    p_representacion_hex = unColor.RepresentacionHexadecimal
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
