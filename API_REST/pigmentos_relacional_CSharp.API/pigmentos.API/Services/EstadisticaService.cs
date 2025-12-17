using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Services
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