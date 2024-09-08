using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.Helpers; // Ensure you include this namespace

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _itemService.GetItemsPagedAndSortedAsync(pageNumber, pageSize);
            return Ok(result);
        }

        // GET: api/Items/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST: api/Items
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            await _itemService.AddItemAsync(item);
            return CreatedAtAction(nameof(GetItemById), new { id = item.ItemId }, item);
        }

        // PUT: api/Items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] Item item)
        {
            if (id != item.ItemId)
            {
                return BadRequest("Item ID mismatch");
            }

            var existingItem = await _itemService.GetItemByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.PurchasePrice = item.PurchasePrice;
            existingItem.SalePrice = item.SalePrice;
            existingItem.Quantity = item.Quantity;
            existingItem.UserId = item.UserId;

            await _itemService.UpdateItemAsync(existingItem);
            return NoContent();
        }

        // DELETE: api/Items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var existingItem = await _itemService.GetItemByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
