using pigmentos.API.Exceptions;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Services
{
    public class ColorService(IColorRepository colorRepository,
                              IPigmentoRepository pigmentoRepository)
    {
        private readonly IColorRepository _colorRepository = colorRepository;
        private readonly IPigmentoRepository _pigmentoRepository = pigmentoRepository;

        public async Task<List<Color>> GetAllAsync()
        {
            var losColores = await _colorRepository
                .GetAllAsync();

            if (losColores.Count == 0)
                throw new EmptyCollectionException("No se encontraron colores registrados");

            return losColores;
        }

        public async Task<Color> GetByIdAsync(string colorId)
        {
            Color unColor = await _colorRepository
                .GetByIdAsync(colorId);

            if (unColor.Id == string.Empty)
                throw new EmptyCollectionException($"Color no encontrado con el Id {colorId}");

            return unColor;
        }

        public async Task<List<Pigmento>> GetAssociatedPigmentsAsync(string colorId)
        {
            Color unColor = await _colorRepository
                .GetByIdAsync(colorId);

            if (unColor.Id == string.Empty)
                throw new EmptyCollectionException($"Color no encontrado con el Id {colorId}");

            var pigmentosAsociados = await _pigmentoRepository
                .GetAllByColorIdAsync(colorId);

            if (pigmentosAsociados.Count == 0)
                throw new EmptyCollectionException($"Color {unColor.Nombre} no tiene pigmentos asociados");

            return pigmentosAsociados;
        }
    }
}
