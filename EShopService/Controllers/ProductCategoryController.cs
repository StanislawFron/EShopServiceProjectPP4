using EShop.Application.Service;
using EShopDomain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _service;

        public ProductCategoryController(IProductCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCategory category)
        {
            var created = await _service.CreateAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategory category)
        {
            if (id != category.Id) return BadRequest();
            var success = await _service.UpdateAsync(category);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] ProductCategory category)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Aktualizuj tylko przekazane pola
            if (!string.IsNullOrEmpty(category.Name))
                existing.Name = category.Name;
            existing.Deleted = category.Deleted;
            existing.UpdatedAt = category.UpdatedAt != default ? category.UpdatedAt : DateTime.UtcNow;
            existing.UpdatedBy = category.UpdatedBy;

            var success = await _service.UpdateAsync(existing);
            return success ? NoContent() : NotFound();
        }
    }    
}
