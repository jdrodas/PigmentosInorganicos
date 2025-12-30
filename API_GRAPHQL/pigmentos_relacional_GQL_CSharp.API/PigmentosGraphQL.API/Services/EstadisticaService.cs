using PigmentosGraphQL.API.Interfaces;
using PigmentosGraphQL.API.Models;

namespace PigmentosGraphQL.API.Services
{
    public class EstadisticaService(IEstadisticaRepository estadisticaRepository)
    {
        private readonly IEstadisticaRepository _estadisticaRepository = estadisticaRepository;

        public async Task<Estadistica> GetAllAsync()
        {
            return await _estadisticaRepository
                .GetAllAsync();
        }
    }
}