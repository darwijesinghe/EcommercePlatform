using SharedLibrary.Enums;

namespace SharedLibrary.DTOs
{
    /// <summary>
    /// DTO model for order.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// The order's unique identifier.
        /// </summary>
        public int? Id                         { get; set; }

        /// <summary>
        /// The user who placing the order.
        /// </summary>
        public int UserId                      { get; set; }

        /// <summary>
        /// The total amount of the order.
        /// </summary>
        public decimal TotalAmount             { get; set; }

        /// <summary>
        /// The current status of the order.
        /// </summary>
        public OrderStatus? Status             { get; set; }

        /// <summary>
        /// The order placed date.
        /// </summary>
        public DateTime? CreatedAt             { get; set; }

        /// <summary>
        /// The items of the order.
        /// </summary>
        public ICollection<OrderItemDto> Items { get; set; }

        public OrderDto()
        {
            Status    = OrderStatus.Pending;
            CreatedAt = DateTime.Now;
            Items     = new List<OrderItemDto>();
        }
    }
}
