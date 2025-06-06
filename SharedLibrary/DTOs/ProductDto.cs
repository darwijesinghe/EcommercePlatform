namespace SharedLibrary.DTOs
{
    /// <summary>
    /// DTO model for product.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// The product's unique identifier.
        /// </summary>
        public int? Id                     { get; set; }

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
        public decimal Price               { get; set; }

        /// <summary>
        /// The initial quantity of the product.
        /// </summary>
        public int Quantity                { get; set; }
    }
}
