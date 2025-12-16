using pigmentos.API.Exceptions;
using pigmentos.API.Interfaces;
using pigmentos.API.Models;

namespace pigmentos.API.Services
{
    public class PigmentoService(IPigmentoRepository pigmentoRepository,
                                 IColorRepository colorRepository,
                                 IFamiliaRepository familiaRepository)
    {
        private readonly IPigmentoRepository _pigmentoRepository = pigmentoRepository;
        private readonly IColorRepository _colorRepository = colorRepository;
        private readonly IFamiliaRepository _familiaRepository = familiaRepository;

        public async Task<List<Pigmento>> GetAllAsync()
        {
            var losPigmentos = await _pigmentoRepository
                .GetAllAsync();

            if (losPigmentos.Count == 0)
                throw new EmptyCollectionException("No se encontraron pigmentos registrados");

            return losPigmentos;
        }

        public async Task<Pigmento> GetByIdAsync(Guid pigmentoId)
        {
            Pigmento unPigmento = await _pigmentoRepository
                .GetByIdAsync(pigmentoId);

            if (unPigmento.Id == Guid.Empty)
                throw new EmptyCollectionException($"Pigmento no encontrado con el Id {pigmentoId}");

            return unPigmento;
        }

        public async Task<Pigmento> CreateAsync(Pigmento unPigmento)
        {
            unPigmento.Nombre = unPigmento.Nombre!.Trim();
            unPigmento.FormulaQuimica = unPigmento.FormulaQuimica!.Trim();
            unPigmento.NumeroCi = unPigmento.NumeroCi!.Trim();

            string resultadoValidacion = EvaluatePigmentDetailsAsync(unPigmento);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var pigmentoExistente = await _pigmentoRepository
                .GetByDetailsAsync(unPigmento);

            if (pigmentoExistente.Nombre == unPigmento.Nombre! &&
                pigmentoExistente.FormulaQuimica == unPigmento.FormulaQuimica! &&
                pigmentoExistente.NumeroCi == pigmentoExistente.NumeroCi!)
                return pigmentoExistente;

            var colorExistente = await _colorRepository
                .GetByIdAsync(unPigmento.ColorId);

            if (colorExistente.Id == Guid.Empty)
                throw new AppValidationException($"No hay color registrado con el id {unPigmento.ColorId}.");

            var familiaExistente = await _familiaRepository
                .GetByIdAsync(unPigmento.FamiliaQuimicaId);

            if (familiaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No hay familia química registrada con el id {unPigmento.FamiliaQuimicaId}.");

            try
            {
                bool resultadoAccion = await _pigmentoRepository
                    .CreateAsync(unPigmento);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                pigmentoExistente = await _pigmentoRepository
                .GetByDetailsAsync(unPigmento);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return pigmentoExistente;
        }

        private static string EvaluatePigmentDetailsAsync(Pigmento unPigmento)
        {
            if (string.IsNullOrEmpty(unPigmento.Nombre))
                return "No se puede insertar un pigmento con nombre nulo";

            if (string.IsNullOrEmpty(unPigmento.FormulaQuimica))
                return "No se puede insertar un pigmento con la fórmula química nula.";

            if (string.IsNullOrEmpty(unPigmento.NumeroCi))
                return "No se puede insertar un pigmento con el número CI nulo.";

            return string.Empty;
        }
    }
}
