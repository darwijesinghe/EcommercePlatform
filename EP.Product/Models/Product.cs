using Microsoft.EntityFrameworkCore;

namespace EP.Product.Models
{
    /// <summary>
    /// Domain model for product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The unique identifier of the product.
        /// </summary>
        public int Id                      { get; set; }

        /// <summary>
        /// The product name.
        /// </summary>
        public required string Name        { get; set; }

        /// <summary>
        /// The product description.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The price of the product.
        /// </summary>
        [Precision(18, 2)]
        public decimal Price               { get; set; }

        /// <summary>
        /// The initial quantity of the product.
        /// </summary>
        public int Quantity                { get; set; }
    }
}
