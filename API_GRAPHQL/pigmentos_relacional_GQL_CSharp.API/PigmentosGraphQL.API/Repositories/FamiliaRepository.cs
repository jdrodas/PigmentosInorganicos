using Dapper;
using Npgsql;
using PigmentosGraphQL.API.DbContexts;
using PigmentosGraphQL.API.Exceptions;
using PigmentosGraphQL.API.Interfaces;
using PigmentosGraphQL.API.Models;
using System.Data;

namespace PigmentosGraphQL.API.Repositories
{
    public class FamiliaRepository(PgsqlDbContext unContexto) : IFamiliaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Familia>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, composicion " +
                "FROM core.familias_quimicas ORDER BY nombre";

            var resultadoFamilias = await conexion
                .QueryAsync<Familia>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoFamilias];
        }

        public async Task<Familia> GetByIdAsync(Guid familiaId)
        {
            Familia unaFamilia = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@familiaId", familiaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, composicion " +
                "FROM core.familias_quimicas " +
                "WHERE id = @familiaId";

            var resultado = await conexion
                .QueryAsync<Familia>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaFamilia = resultado.First();

            return unaFamilia;
        }

        public async Task<Familia> GetByDetailsAsync(Familia unaFamilia)
        {
            Familia familiaExistente = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@familiaNombre", unaFamilia.Nombre,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@familiaComposicion", unaFamilia.Composicion,
                        DbType.String, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, composicion " +
                "FROM core.familias_quimicas " +
                "WHERE LOWER(nombre) = LOWER(@familiaNombre) " +
                "AND LOWER(composicion) = LOWER(@familiaComposicion)";


            var resultado = await conexion
                .QueryAsync<Familia>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                familiaExistente = resultado.First();

            return familiaExistente;
        }

        public async Task<bool> CreateAsync(Familia unaFamilia)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_familia_quimica";
                var parametros = new
                {
                    p_nombre = unaFamilia.Nombre,
                    p_composicion = unaFamilia.Composicion
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

        public async Task<bool> UpdateAsync(Familia unaFamilia)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_familia_quimica";
                var parametros = new
                {
                    p_id = unaFamilia.Id,
                    p_nombre = unaFamilia.Nombre,
                    p_composicion = unaFamilia.Composicion
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

        public async Task<bool> RemoveAsync(Guid familiaId)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_familia_quimica";
                var parametros = new
                {
                    p_id = familiaId
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
