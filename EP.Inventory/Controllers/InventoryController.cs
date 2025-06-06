using EP.Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EP.Inventory.Controllers
{
    /// <summary>
    /// Handles inventory-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        // Services
        private readonly IInventoryService _invService;

        public InventoryController(IInventoryService invService)
        {
            _invService = invService;
        }

        /// <summary>
        /// Retrieves all the inventory data in the system.
        /// </summary>
        [HttpGet("get-inventory")]
        public async Task<JsonResult> GetInventory()
        {
            try
            {
                // gets the result
                var result = await _invService.GetInventoryAsync();

                return new JsonResult(new { result.Success, result.Message, result.Data });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }
    }
}
