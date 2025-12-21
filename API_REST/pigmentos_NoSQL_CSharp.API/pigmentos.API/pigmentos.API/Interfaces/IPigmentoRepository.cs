using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IPigmentoRepository
    {
        public Task<List<Pigmento>> GetAllAsync();
        public Task<Pigmento> GetByIdAsync(string pigmentoId);
        public Task<Pigmento> GetByDetailsAsync(Pigmento unPigmento);
        public Task<List<Pigmento>> GetAllByColorIdAsync(string colorId);
        public Task<List<Pigmento>> GetAllByFamilyIdAsync(string familiaId);

        public Task<bool> CreateAsync(Pigmento unPigmento);
    }
}
