using pigmentos.API.Exceptions;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Services
{
    public class FamiliaService(IFamiliaRepository familiaRepository,
                              IPigmentoRepository pigmentoRepository)
    {
        private readonly IFamiliaRepository _familiaRepository = familiaRepository;
        private readonly IPigmentoRepository _pigmentoRepository = pigmentoRepository;

        public async Task<List<Familia>> GetAllAsync()
        {
            var lasFamilias = await _familiaRepository
                .GetAllAsync();

            if (lasFamilias.Count == 0)
                throw new EmptyCollectionException("No se encontraron familias registradas");

            return lasFamilias;
        }

        public async Task<Familia> GetByIdAsync(Guid familiaId)
        {
            Familia unaFamilia = await _familiaRepository
                .GetByIdAsync(familiaId);

            if (unaFamilia.Id == Guid.Empty)
                throw new EmptyCollectionException($"Familia Química no encontrada con el Id {familiaId}");

            return unaFamilia;
        }

        public async Task<List<Pigmento>> GetAssociatedPigmentsAsync(Guid familiaId)
        {
            Familia unaFamilia = await _familiaRepository
                .GetByIdAsync(familiaId);

            if (unaFamilia.Id == Guid.Empty)
                throw new EmptyCollectionException($"Familia Química no encontrada con el Id {familiaId}");

            var pigmentosAsociados = await _pigmentoRepository
                .GetAllByFamilyIdAsync(familiaId);

            if (pigmentosAsociados.Count == 0)
                throw new EmptyCollectionException($"Familia Química {unaFamilia.Nombre} no tiene pigmentos asociados");

            return pigmentosAsociados;
        }
    }
}