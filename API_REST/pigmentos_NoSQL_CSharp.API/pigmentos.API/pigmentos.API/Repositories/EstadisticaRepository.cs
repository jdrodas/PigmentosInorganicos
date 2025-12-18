using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Repositories
{
    public class EstadisticaRepository(MongoDbContext unContexto) : IEstadisticaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            Estadistica unaEstadistica = new();
            var conexion = contextoDB
                .CreateConnection();

            var coleccionColores = conexion
                .GetCollection<Color>(contextoDB.ConfiguracionColecciones.ColeccionColores);

            var totalColores = await coleccionColores
                .EstimatedDocumentCountAsync();

            unaEstadistica.Colores = totalColores;

            var coleccionFamiliasQuimicas = conexion
                .GetCollection<Familia>(contextoDB.ConfiguracionColecciones.ColeccionFamiliasQuimicas);

            var totalFamilias = await coleccionFamiliasQuimicas
                .EstimatedDocumentCountAsync();

            unaEstadistica.FamiliasQuimicas = totalFamilias;

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var totalEventos = await coleccionPigmentos
                .EstimatedDocumentCountAsync();

            unaEstadistica.Pigmentos = totalEventos;

            return unaEstadistica;
        }
    }
}