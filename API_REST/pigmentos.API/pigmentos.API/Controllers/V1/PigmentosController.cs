using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using pigmentos.API.Exceptions;
using pigmentos.API.Models;
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

        [HttpGet("{pigmentoId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid pigmentoId)
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Pigmento unPigmento)
        {
            try
            {
                var pigmentoCreado = await _pigmentoService
                    .CreateAsync(unPigmento);

                return Ok(pigmentoCreado);
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
    }
}
