using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IFamiliaRepository
    {
        public Task<List<Familia>> GetAllAsync();
        public Task<Familia> GetByIdAsync(string familiaId);
        public Task<Familia> GetByDetailsAsync(Familia familiaId);
        public Task<bool> CreateAsync(Familia unaFmailia);
        public Task<bool> UpdateAsync(Familia unaFmailia);
    }
}
