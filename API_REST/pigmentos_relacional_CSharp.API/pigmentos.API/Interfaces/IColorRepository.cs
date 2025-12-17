using pigmentos.API.Models;

namespace pigmentos.API.Interfaces
{
    public interface IColorRepository
    {
        public Task<List<Color>> GetAllAsync();
        public Task<Color> GetByIdAsync(Guid colorId);
        public Task<Color> GetByDetailsAsync(Color unColor);
        public Task<bool> CreateAsync(Color unColor);
        public Task<bool> UpdateAsync(Color unColor);
        public Task<bool> RemoveAsync(Guid colorId);
    }
}
