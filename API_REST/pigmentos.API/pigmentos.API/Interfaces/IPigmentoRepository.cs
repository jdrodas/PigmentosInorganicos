using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IPigmentoRepository
    {
        public Task<List<PigmentoDetallado>> GetAllAsync();
        public Task<PigmentoDetallado> GetByIdAsync(Guid pigmentoId);
        public Task<List<Pigmento>> GetAllByColorIdAsync(Guid colorId);
        public Task<List<Pigmento>> GetAllByFamilyIdAsync(Guid familiaId);
    }
}
