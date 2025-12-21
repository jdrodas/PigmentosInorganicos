using pigmentos.API.Exceptions;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Services
{
    public class PigmentoService(IPigmentoRepository pigmentoRepository)
    {
        private readonly IPigmentoRepository _pigmentoRepository = pigmentoRepository;

        public async Task<List<Pigmento>> GetAllAsync()
        {
            var losPigmentos = await _pigmentoRepository
                .GetAllAsync();

            if (losPigmentos.Count == 0)
                throw new EmptyCollectionException("No se encontraron pigmentos registrados");

            return losPigmentos;
        }

        public async Task<Pigmento> GetByIdAsync(string pigmentoId)
        {
            Pigmento unPigmento = await _pigmentoRepository
                .GetByIdAsync(pigmentoId);

            if (unPigmento.Id == string.Empty)
                throw new EmptyCollectionException($"Pigmento no encontrado con el Id {pigmentoId}");

            return unPigmento;
        }
    }
}
