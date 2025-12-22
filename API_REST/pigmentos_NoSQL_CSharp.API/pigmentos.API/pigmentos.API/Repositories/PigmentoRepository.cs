using MongoDB.Driver;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Repositories
{
    public class PigmentoRepository(MongoDbContext unContexto) : IPigmentoRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Pigmento>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var losPigmentos = await coleccionPigmentos
                .Find(_ => true)
                .SortBy(pigmento => pigmento.Nombre)
                .ToListAsync();

            return losPigmentos;
        }

        public async Task<Pigmento> GetByIdAsync(string pigmentoId)
        {
            Pigmento unPigmento = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var resultado = await coleccionPigmentos
                .Find(pigmento => pigmento.Id == pigmentoId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unPigmento = resultado;

            return unPigmento;
        }

        public async Task<Pigmento> GetByDetailsAsync(Pigmento unPigmento)
        {
            Pigmento pigmentoExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var builder = Builders<Pigmento>.Filter;
            var filtro = builder.And(
                builder.Regex(pigmento => pigmento.Nombre, $"/^{unPigmento.Nombre}$/i"),
                builder.Regex(pigmento => pigmento.FormulaQuimica, $"/^{unPigmento.FormulaQuimica}$/i"),
                builder.Regex(pigmento => pigmento.NumeroCi, $"/^{unPigmento.NumeroCi}$/i")
                );

            var resultado = await coleccionPigmentos
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                pigmentoExistente = resultado;

            return pigmentoExistente;
        }

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

        public async Task<List<Pigmento>> GetAllByFamilyIdAsync(string familiaId)
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var losPigmentos = await coleccionPigmentos
                .Find(pigmento => pigmento.FamiliaQuimicaId == familiaId)
                .SortBy(pigmento => pigmento.Nombre)
                .ToListAsync();

            return losPigmentos;
        }

        public async Task<bool> CreateAsync(Pigmento unPigmento)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            await coleccionPigmentos
                .InsertOneAsync(unPigmento);

            var resultado = await GetByDetailsAsync(unPigmento);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Pigmento unPigmento)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var resultado = await coleccionPigmentos
                .ReplaceOneAsync(pigmento => pigmento.Id == unPigmento.Id, unPigmento);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(string pigmentoId)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionPigmentos = conexion
                .GetCollection<Pigmento>(contextoDB.ConfiguracionColecciones.ColeccionPigmentos);

            var resultado = await coleccionPigmentos
                .DeleteOneAsync(pigmento => pigmento.Id == pigmentoId);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
