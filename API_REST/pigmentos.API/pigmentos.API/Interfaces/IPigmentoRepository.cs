using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IPigmentoRepository
    {
        public Task<List<Pigmento>> GetAllAsync();
        public Task<Pigmento> GetByIdAsync(Guid pigmentoId);
        public Task<Pigmento> GetByDetailsAsync(Pigmento unPigmento);
        public Task<List<Pigmento>> GetAllByColorIdAsync(Guid colorId);
        public Task<List<Pigmento>> GetAllByFamilyIdAsync(Guid familiaId);
        public Task<bool> CreateAsync(Pigmento unPigmento);
        public Task<bool> UpdateAsync(Pigmento unPigmento);
    }
}
