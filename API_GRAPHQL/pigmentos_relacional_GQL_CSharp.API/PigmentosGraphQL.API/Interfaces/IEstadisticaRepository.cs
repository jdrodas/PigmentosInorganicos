using PigmentosGraphQL.API.Models;

namespace PigmentosGraphQL.API.Interfaces
{
    public interface IEstadisticaRepository
    {
        public Task<Estadistica> GetAllAsync();
    }
}
