using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IColorRepository
    {
        public Task<List<Color>> GetAllAsync();
        public Task<Color> GetByIdAsync(Guid colorId);
    }
}
