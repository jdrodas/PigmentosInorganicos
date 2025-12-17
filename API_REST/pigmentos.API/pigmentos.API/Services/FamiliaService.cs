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

        public async Task<Familia> CreateAsync(Familia unaFamilia)
        {
            unaFamilia.Nombre = unaFamilia.Nombre!.Trim();
            unaFamilia.Composicion = unaFamilia.Composicion!.Trim();

            string resultadoValidacion = EvaluateFamilyDetailsAsync(unaFamilia);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var familiaExistente = await _familiaRepository
                .GetByDetailsAsync(unaFamilia);

            if (familiaExistente.Nombre == unaFamilia.Nombre! &&
                familiaExistente.Composicion == unaFamilia.Composicion)
                return familiaExistente;

            try
            {
                bool resultadoAccion = await _familiaRepository
                    .CreateAsync(unaFamilia);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                familiaExistente = await _familiaRepository
                .GetByDetailsAsync(unaFamilia);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return familiaExistente;
        }

        public async Task<Familia> UpdateAsync(Familia unaFamilia)
        {
            unaFamilia.Nombre = unaFamilia.Nombre!.Trim();
            unaFamilia.Composicion = unaFamilia.Composicion!.Trim();

            string resultadoValidacion = EvaluateFamilyDetailsAsync(unaFamilia);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var familiaExistente = await _familiaRepository
                .GetByIdAsync(unaFamilia.Id);

            if (familiaExistente.Id == Guid.Empty)
                throw new EmptyCollectionException($"No existe una familia química con el Guid {unaFamilia.Id} que se pueda actualizar");

            if (familiaExistente.Equals(unaFamilia))
                return familiaExistente;

            try
            {
                bool resultadoAccion = await _familiaRepository
                    .UpdateAsync(unaFamilia);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                familiaExistente = await _familiaRepository
                    .GetByIdAsync(unaFamilia.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return familiaExistente;
        }

        public async Task<string> RemoveAsync(Guid familiaId)
        {
            Familia unaFamilia = await _familiaRepository
                .GetByIdAsync(familiaId);

            if (unaFamilia.Id == Guid.Empty)
                throw new EmptyCollectionException($"Familia ´química no encontrada con el id {familiaId}");

            var pigmentosAsociados = await _pigmentoRepository
                .GetAllByFamilyIdAsync(familiaId);

            if (pigmentosAsociados.Count != 0)
                throw new AppValidationException($"La familia química {unaFamilia.Nombre} no se puede eliminar porque tiene {pigmentosAsociados.Count} pigmentos asociados");

            string nombreFamiliaQuimicaEliminada = unaFamilia.Nombre!;

            try
            {
                bool resultadoAccion = await _familiaRepository
                    .RemoveAsync(familiaId);

                if (!resultadoAccion)
                    throw new DbOperationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException)
            {
                throw;
            }

            return nombreFamiliaQuimicaEliminada;
        }

        private static string EvaluateFamilyDetailsAsync(Familia unaFamilia)
        {
            if (string.IsNullOrEmpty(unaFamilia.Nombre))
                return "No se puede insertar una familia química con nombre nulo";

            if (string.IsNullOrEmpty(unaFamilia.Composicion))
                return "No se puede insertar una familia química con la composición nula.";

            return string.Empty;
        }
    }
}