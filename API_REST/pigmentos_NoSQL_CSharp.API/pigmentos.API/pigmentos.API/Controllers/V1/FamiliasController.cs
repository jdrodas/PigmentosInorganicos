using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using pigmentos.API.Exceptions;
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

        [HttpGet("{familiaId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string familiaId)
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

        [HttpGet("{familiaId:length(24)}/pigmentos")]
        public async Task<IActionResult> GetAssociatedPigmentsAsync(string familiaId)
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
    }
}
