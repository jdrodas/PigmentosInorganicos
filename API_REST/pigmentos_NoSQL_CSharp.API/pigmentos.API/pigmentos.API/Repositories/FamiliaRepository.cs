using MongoDB.Driver;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Repositories
{
    public class FamiliaRepository(MongoDbContext unContexto) : IFamiliaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Familia>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccioFamilias = conexion
                .GetCollection<Familia>(contextoDB.ConfiguracionColecciones.ColeccionFamiliasQuimicas);

            var lasFamilias = await coleccioFamilias
                .Find(_ => true)
                .SortBy(familia => familia.Nombre)
                .ToListAsync();

            return lasFamilias;
        }

        public async Task<Familia> GetByIdAsync(string familiaId)
        {
            Familia unaFamilia = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccioFamilias = conexion
                .GetCollection<Familia>(contextoDB.ConfiguracionColecciones.ColeccionFamiliasQuimicas);

            var resultado = await coleccioFamilias
                .Find(familia => familia.Id == familiaId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaFamilia = resultado;

            return unaFamilia;
        }

        public async Task<Familia> GetByDetailsAsync(Familia unColor)
        {
            Familia familiaExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccioFamilias = conexion
                .GetCollection<Familia>(contextoDB.ConfiguracionColecciones.ColeccionFamiliasQuimicas);

            var builder = Builders<Familia>.Filter;
            var filtro = builder.And(
                builder.Regex(color => color.Nombre, $"/^{unColor.Nombre}$/i"),
                builder.Regex(color => color.Composicion, $"/^{unColor.Composicion}$/i")
                );

            var resultado = await coleccioFamilias
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                familiaExistente = resultado;

            return familiaExistente;
        }
    }
}
