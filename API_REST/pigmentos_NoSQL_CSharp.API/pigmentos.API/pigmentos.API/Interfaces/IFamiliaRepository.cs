using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IFamiliaRepository
    {
        public Task<List<Familia>> GetAllAsync();
        public Task<Familia> GetByIdAsync(string familiaId);
        public Task<Familia> GetByDetailsAsync(Familia familiaId);
    }
}
