using Dapper;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Repositories
{
    public class EstadisticaRepository(PgsqlDbContext unContexto) : IEstadisticaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            Estadistica conteoRegistros = new();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.colores";

            conteoRegistros.TotalColores = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(id) total FROM core.familias_quimicas";

            conteoRegistros.TotalFamiliasQuimicas = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(id) total FROM core.pigmentos";

            conteoRegistros.TotalPigmentos = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            return conteoRegistros;
        }
    }
}