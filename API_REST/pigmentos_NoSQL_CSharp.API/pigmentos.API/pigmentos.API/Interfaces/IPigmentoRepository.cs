using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IPigmentoRepository
    {
        public Task<List<Pigmento>> GetAllAsync();
        public Task<Pigmento> GetByIdAsync(string pigmentoId);
        public Task<List<Pigmento>> GetAllByColorIdAsync(string colorId);
        public Task<List<Pigmento>> GetAllByFamilyIdAsync(string familiaId);
    }
}
