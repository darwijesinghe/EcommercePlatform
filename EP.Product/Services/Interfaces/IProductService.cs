using SharedLibrary.DTOs;
using SharedLibrary.Response;

namespace EP.Product.Services.Interfaces
{
    /// <summary>
    /// Defines contracts of product related functionalities.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{IEnumerable{ProductDto}}"/> containing a list of products.
        /// </returns>
        Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();

        /// <summary>
        /// Retrieves product by its PK.
        /// </summary>
        /// <param name="id">The PK of the product.</param>
        /// <returns>
        /// A <see cref="Result{ProductDto}"/> containing a data of the product.
        /// </returns>
        Task<Result<ProductDto>> GetProductAsync(int id);

        /// <summary>
        /// Adds a new product to the database and publishing order data to RabbitMQ message broker
        /// </summary>
        /// <param name="data">The product data to be stored.</param>
        /// <returns>
        /// A <see cref="Result"/> that contains the product creation process result.
        /// </returns>
        Task<Result> AddProductAsync(ProductDto data);
    }
}
