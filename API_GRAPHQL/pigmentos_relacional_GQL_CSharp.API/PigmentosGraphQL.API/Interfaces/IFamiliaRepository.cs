using PigmentosGraphQL.API.Models;

namespace PigmentosGraphQL.API.Interfaces
{
    public interface IFamiliaRepository
    {
        public Task<List<Familia>> GetAllAsync();
        public Task<Familia> GetByIdAsync(Guid familiaId);
        public Task<Familia> GetByDetailsAsync(Familia familiaId);
        public Task<bool> CreateAsync(Familia unaFamilia);
        public Task<bool> UpdateAsync(Familia unaFamilia);
        public Task<bool> RemoveAsync(Guid familiaId);
    }
}
