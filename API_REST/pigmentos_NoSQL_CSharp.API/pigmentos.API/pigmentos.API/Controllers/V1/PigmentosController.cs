using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using pigmentos.API.Exceptions;
using pigmentos.API.Services;

namespace pigmentos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/pigmentos")]
    public class PigmentosController(PigmentoService pigmentoService) : Controller
    {
        private readonly PigmentoService _pigmentoService = pigmentoService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var losPigmentos = await _pigmentoService
                .GetAllAsync();

                return Ok(losPigmentos);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{pigmentoId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string pigmentoId)
        {
            try
            {
                var unPigmento = await _pigmentoService
                    .GetByIdAsync(pigmentoId);

                return Ok(unPigmento);
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
