using MongoDB.Driver;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Repositories
{
    public class PigmentoRepository(MongoDbContext unContexto) : IPigmentoRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Pigmento>> GetAllByColorIdAsync(string colorId)
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var losPigmentos = await coleccionPigmentos
                .Find(pigmento => pigmento.ColorId == colorId)
                .SortBy(pigmento => pigmento.Nombre)
                .ToListAsync();

            return losPigmentos;
        }
    }
}
