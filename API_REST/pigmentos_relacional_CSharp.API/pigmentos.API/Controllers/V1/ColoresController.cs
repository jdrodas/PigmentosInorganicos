using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using pigmentos.API.Exceptions;
using pigmentos.API.Models;
using pigmentos.API.Services;

namespace pigmentos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/colores")]
    public class ColoresController(ColorService colorService) : Controller
    {
        private readonly ColorService _colorService = colorService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var losColores = await _colorService
                .GetAllAsync();

                return Ok(losColores);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{colorId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid colorId)
        {
            try
            {
                var unColor = await _colorService
                    .GetByIdAsync(colorId);

                return Ok(unColor);
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

        [HttpGet("{colorId:Guid}/pigmentos")]
        public async Task<IActionResult> GetAssociatedPigmentsAsync(Guid colorId)
        {
            try
            {
                var losPigmentosAsociados = await _colorService
                    .GetAssociatedPigmentsAsync(colorId);

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
        public async Task<IActionResult> CreateAsync(Color unColor)
        {
            try
            {
                var colorCreado = await _colorService
                    .CreateAsync(unColor);

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
        public async Task<IActionResult> UpdateAsync(Color unColor)
        {
            try
            {
                var colorActualizado = await _colorService
                    .UpdateAsync(unColor);

                return Ok(colorActualizado);
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

        [HttpDelete("{colorId:Guid}")]
        public async Task<IActionResult> RemoveAsync(Guid colorId)
        {
            try
            {
                var nombreColorBorrado = await _colorService
                    .RemoveAsync(colorId);

                return Ok($"El color {nombreColorBorrado} fue eliminado correctamente!");
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
