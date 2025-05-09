using InventoryApp.Service.DTO;
using InventoryApp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
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
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            await _service.AddAsync(dto);
            return Ok("Product added successfully");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] CategoryDto dto)
        {
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
