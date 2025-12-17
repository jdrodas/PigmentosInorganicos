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

        public async Task<Color> GetByIdAsync(Guid colorId)
        {
            Color unColor = await _colorRepository
                .GetByIdAsync(colorId);

            if (unColor.Id == Guid.Empty)
                throw new EmptyCollectionException($"Color no encontrado con el Id {colorId}");

            return unColor;
        }

        public async Task<List<Pigmento>> GetAssociatedPigmentsAsync(Guid colorId)
        {
            Color unColor = await _colorRepository
                .GetByIdAsync(colorId);

            if (unColor.Id == Guid.Empty)
                throw new EmptyCollectionException($"Color no encontrado con el Id {colorId}");

            var pigmentosAsociados = await _pigmentoRepository
                .GetAllByColorIdAsync(colorId);

            if (pigmentosAsociados.Count == 0)
                throw new EmptyCollectionException($"Color {unColor.Nombre} no tiene pigmentos asociados");

            return pigmentosAsociados;
        }

        public async Task<Color> CreateAsync(Color unColor)
        {
            unColor.Nombre = unColor.Nombre!.Trim();
            unColor.RepresentacionHexadecimal = unColor.RepresentacionHexadecimal!.Trim();

            string resultadoValidacion = EvaluateColorDetailsAsync(unColor);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var colorExistente = await _colorRepository
                .GetByDetailsAsync(unColor);

            if (colorExistente.Nombre == unColor.Nombre! &&
                colorExistente.RepresentacionHexadecimal == unColor.RepresentacionHexadecimal)
                return colorExistente;

            try
            {
                bool resultadoAccion = await _colorRepository
                    .CreateAsync(unColor);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                colorExistente = await _colorRepository
                .GetByDetailsAsync(unColor);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return colorExistente;
        }

        public async Task<Color> UpdateAsync(Color unColor)
        {
            unColor.Nombre = unColor.Nombre!.Trim();
            unColor.RepresentacionHexadecimal = unColor.RepresentacionHexadecimal!.Trim();

            string resultadoValidacion = EvaluateColorDetailsAsync(unColor);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var colorExistente = await _colorRepository
                .GetByIdAsync(unColor.Id);

            if (colorExistente.Id == Guid.Empty)
                throw new EmptyCollectionException($"No existe un color con el Guid {unColor.Id} que se pueda actualizar");

            if (colorExistente.Equals(unColor))
                return colorExistente;

            try
            {
                bool resultadoAccion = await _colorRepository
                    .UpdateAsync(unColor);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                colorExistente = await _colorRepository
                    .GetByIdAsync(unColor.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return colorExistente;
        }

        public async Task<string> RemoveAsync(Guid colorId)
        {
            Color unColor = await _colorRepository
                .GetByIdAsync(colorId);

            if (unColor.Id == Guid.Empty)
                throw new EmptyCollectionException($"Color no encontrado con el id {colorId}");

            var pigmentosAsociados = await _pigmentoRepository
                .GetAllByColorIdAsync(colorId);

            if (pigmentosAsociados.Count != 0)
                throw new AppValidationException($"El color {unColor.Nombre} no se puede eliminar porque tiene {pigmentosAsociados.Count} pigmentos asociados");

            string nombreColorEliminado = unColor.Nombre!;

            try
            {
                bool resultadoAccion = await _colorRepository
                    .RemoveAsync(colorId);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombreColorEliminado;
        }

        private static string EvaluateColorDetailsAsync(Color unColor)
        {
            if (string.IsNullOrEmpty(unColor.Nombre))
                return "No se puede insertar un color con nombre nulo";

            if (string.IsNullOrEmpty(unColor.RepresentacionHexadecimal))
                return "No se puede insertar un color con la representación hexadecimal nula.";

            return string.Empty;
        }
    }
}