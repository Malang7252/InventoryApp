using FluentValidation;
using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IValidator<ProductDto> _validator;

        public ProductController(IProductService service, IValidator<ProductDto> validator)
        {
            _service = service;
            _validator = validator;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
        {
            return Ok(await _service.GetAll(categoryId));
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            return Ok(await _service.GetById(id));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                return BadRequest(new { Errors = errorResponse });
            }

            await _service.AddAsync(dto);
            return Ok("Product added successfully");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] ProductDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                });
                return BadRequest(new { Errors = errorResponse });
            }
            await _service.Update(id, dto);
            return Ok("Product updated successfully");
        }
        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _service.Delete(id);
            return Ok("Product deleted successfully");
        }
    }
}
