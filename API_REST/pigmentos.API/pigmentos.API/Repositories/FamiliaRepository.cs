using Dapper;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;
using System.Data;

namespace pigmentos.API.Repositories
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
                "WHERE LOWER(nombr) = LOWER(@familiaNombre) " +
                "AND LOWER(composicion) = LOWER(@familiaComposicion)";


            var resultado = await conexion
                .QueryAsync<Familia>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                familiaExistente = resultado.First();

            return familiaExistente;
        }
    }
}
