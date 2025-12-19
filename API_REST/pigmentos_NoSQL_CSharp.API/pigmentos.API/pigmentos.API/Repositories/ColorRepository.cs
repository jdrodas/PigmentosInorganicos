using MongoDB.Driver;
using pigmentos.API.DbContexts;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Repositories
{
    public class ColorRepository(MongoDbContext unContexto) : IColorRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Color>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionColores = conexion
                .GetCollection<Color>(contextoDB.ConfiguracionColecciones.ColeccionColores);

            var losColores = await coleccionColores
                .Find(_ => true)
                .SortBy(color => color.Nombre)
                .ToListAsync();

            return losColores;
        }

        public async Task<Color> GetByIdAsync(string colorId)
        {
            Color unColor = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionColores = conexion
                .GetCollection<Color>(contextoDB.ConfiguracionColecciones.ColeccionColores);

            var resultado = await coleccionColores
                .Find(color => color.Id == colorId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unColor = resultado;

            return unColor;
        }

        public async Task<Color> GetByDetailsAsync(Color unColor)
        {
            Color colorExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionColores = conexion
                .GetCollection<Color>(contextoDB.ConfiguracionColecciones.ColeccionColores);

            var builder = Builders<Color>.Filter;
            var filtro = builder.And(
                builder.Regex(color => color.Nombre, $"/^{unColor.Nombre}$/i"),
                builder.Regex(color => color.RepresentacionHexadecimal, $"/^{unColor.RepresentacionHexadecimal}$/i")
                );

            var resultado = await coleccionColores
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                colorExistente = resultado;

            return colorExistente;
        }

        public async Task<bool> CreateAsync(Color unColor)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionColores = conexion
                .GetCollection<Color>(contextoDB.ConfiguracionColecciones.ColeccionColores);

            await coleccionColores
                .InsertOneAsync(unColor);

            var resultado = await GetByDetailsAsync(unColor);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
