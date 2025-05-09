using System.Security.Claims;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IValidator<CategoryDto> _validator;

        public CategoryController(ICategoryService service, IValidator<CategoryDto> validator)
        {
            _service = service;
            _validator = validator;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
        {
            if (GetUserIdFromToken() != 1)
            {
                return Unauthorized("You are not authorized to delete this product");
            }
            return Ok(await _service.GetAll(categoryId));
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            if (GetUserIdFromToken() != 1)
            {
                return Unauthorized("You are not authorized to delete this product");
            }
            return Ok(await _service.GetById(id));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            if (GetUserIdFromToken() != 1)
            {
                return Unauthorized("You are not authorized to delete this product");
            }
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
            var result = await _service.AddAsync(dto);
            return Ok("Category added successfully");
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] CategoryDto dto)
        {
            if (GetUserIdFromToken() != 1)
            {
                return Unauthorized("You are not authorized to delete this product");
            }
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
            var result = await _service.Update(id, dto);
            if (!result)
            {
                return NotFound(new { message = "Category not found." });
            }
            return Ok("Category updated successfully");
        }
        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (GetUserIdFromToken() != 1)
            {
                return Unauthorized("You are not authorized to delete this product");
            }
            var result = await _service.Delete(id);
            if (!result)
            {
                return NotFound(new { message = "Category not found." });
            }
            return Ok("Category deleted successfully");
        }
        protected long GetUserIdFromToken()
        {
            long UserId = 0;
            try
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var identity = HttpContext.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        IEnumerable<Claim> claims = identity.Claims;
                        string strUserId = identity.FindFirst("UserId").Value;
                        long.TryParse(strUserId, out UserId);
                    }
                }
                return UserId;
            }
            catch
            {
                return UserId;
            }
        }
    }
}
