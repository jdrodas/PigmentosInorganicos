using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IFamiliaRepository
    {
        public Task<List<Familia>> GetAllAsync();
        public Task<Familia> GetByIdAsync(string familiaId);
        public Task<Familia> GetByDetailsAsync(Familia familiaId);
        public Task<bool> CreateAsync(Familia unaFamilia);
        public Task<bool> UpdateAsync(Familia unaFamilia);
        public Task<bool> RemoveAsync(string familiaId);
    }
}
