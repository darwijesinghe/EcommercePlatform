using EP.Order.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;

namespace EP.Order.Controllers
{
    /// <summary>
    /// Handles order-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Services
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order to the database.
        /// </summary>
        /// <param name="data">The type of <see cref="OrderDto"/> that contains the order data.</param>
        [HttpPost("create-order")]
        public async Task<JsonResult> CreateNewOrder([FromBody] OrderDto data)
        {
            try
            {
                // validations
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Required data is not found." });

                // gets the result
                var result = await _orderService.CreateOrderAsync(data);

                return new JsonResult(new { result.Success, result.Message });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all the orders in the system.
        /// </summary>
        [HttpGet("get-all-orders")]
        public async Task<JsonResult> GetAllOrders()
        {
            try
            {
                // gets the result
                var result = await _orderService.GetAllOrdersAsync();

                return new JsonResult(new { result.Success, result.Message, result.Data });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a order by its id.
        /// </summary>
        /// <param name="id">The unique identifier of the order</param>
        [HttpGet("get-a-order")]
        public async Task<JsonResult> GetOrder(int id)
        {
            try
            {
                // gets the result
                var result = await _orderService.GetOrderAsync(id);

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
