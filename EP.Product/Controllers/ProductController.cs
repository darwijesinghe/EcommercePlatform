using EP.Product.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;

namespace EP.Product.Controllers
{
    /// <summary>
    /// Handles product-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Services
        private readonly IProductService _prodService;

        public ProductController(IProductService prodService)
        {
            _prodService = prodService;
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="data">The type of <see cref="ProductDto"/> that contains the product data.</param>
        [HttpPost("add-product")]
        public async Task<JsonResult> AddNewProduct([FromBody] ProductDto data)
        {
            try
            {
                // validations
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, message = "Required data is not found." });

                // gets the result
                var result = await _prodService.AddProductAsync(data);

                return new JsonResult(new { result.Success, result.Message });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all the products in the system.
        /// </summary>
        [HttpGet("get-all-products")]
        public async Task<JsonResult> GetAllProducts()
        {
            try
            {
                // gets the result
                var result = await _prodService.GetAllProductsAsync();

                return new JsonResult(new { result.Success, result.Message, result.Data });
            }
            catch (Exception ex)
            {
                // returns the error
                return new JsonResult(new { ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a products by its id.
        /// </summary>
        /// <param name="id">The PK of the product.</param>
        [HttpGet("get-a-product")]
        public async Task<JsonResult> GetProduct(int id)
        {
            try
            {
                // gets the result
                var result = await _prodService.GetProductAsync(id);

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
