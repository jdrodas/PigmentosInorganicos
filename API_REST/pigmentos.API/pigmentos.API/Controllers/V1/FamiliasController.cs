using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using pigmentos.API.Exceptions;
using pigmentos.API.Models;
using pigmentos.API.Services;

namespace pigmentos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/familias")]
    public class FamiliasController(FamiliaService familiaService) : Controller
    {
        private readonly FamiliaService _familiaService = familiaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var lasFamilias = await _familiaService
                .GetAllAsync();

                return Ok(lasFamilias);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{familiaId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid familiaId)
        {
            try
            {
                var unaFamilia = await _familiaService
                    .GetByIdAsync(familiaId);

                return Ok(unaFamilia);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{familiaId:Guid}/pigmentos")]
        public async Task<IActionResult> GetAssociatedPigmentsAsync(Guid familiaId)
        {
            try
            {
                var losPigmentosAsociados = await _familiaService
                    .GetAssociatedPigmentsAsync(familiaId);

                return Ok(losPigmentosAsociados);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Familia unaFamilia)
        {
            try
            {
                var colorCreado = await _familiaService
                    .CreateAsync(unaFamilia);

                return Ok(colorCreado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Familia unaFamilia)
        {
            try
            {
                var familiaActualizada = await _familiaService
                    .UpdateAsync(unaFamilia);

                return Ok(familiaActualizada);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpDelete("{familiaId:Guid}")]
        public async Task<IActionResult> RemoveAsync(Guid familiaId)
        {
            try
            {
                var nombreFamiliaBorrada = await _familiaService
                    .RemoveAsync(familiaId);

                return Ok($"La familia {nombreFamiliaBorrada} fue eliminada correctamente!");
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}