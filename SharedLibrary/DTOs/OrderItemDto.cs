namespace SharedLibrary.DTOs
{
    /// <summary>
    /// DTO model for order items.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// The ordered item's unique identifier.
        /// </summary>
        public int? Id           { get; set; }

        /// <summary>
        /// The order id.
        /// </summary>
        public int? OrderId      { get; set; }

        /// <summary>
        /// The id of the ordered product.
        /// </summary>
        public int ProductId     { get; set; }

        /// <summary>
        /// The quantity of the ordered item.
        /// </summary>
        public int Quantity      { get; set; }

        /// <summary>
        /// The unit price of the ordered product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
