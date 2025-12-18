using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IEstadisticaRepository
    {
        public Task<Estadistica> GetAllAsync();
    }
}