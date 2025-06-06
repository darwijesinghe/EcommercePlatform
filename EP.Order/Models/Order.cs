using Microsoft.EntityFrameworkCore;
using SharedLibrary.Enums;

namespace EP.Order.Models
{
    /// <summary>
    /// Domain model for order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The unique identifier of the order.
        /// </summary>
        public int Id                       { get; set; }

        /// <summary>
        /// The user who placing the order.
        /// </summary>
        public int UserId                   { get; set; }

        /// <summary>
        /// The total amount of the order.
        /// </summary>
        [Precision(18, 2)]
        public decimal TotalAmount          { get; set; }

        /// <summary>
        /// The current status of the order.
        /// </summary>
        public OrderStatus Status           { get; set; }

        /// <summary>
        /// The order placed date.
        /// </summary>
        public DateTime CreatedAt           { get; set; }

        /// <summary>
        /// The items of the order.
        /// </summary>
        public ICollection<OrderItem> Items { get; set; }

        public Order()
        {
            Status    = OrderStatus.Pending;
            CreatedAt = DateTime.Now;
            Items     = new List<OrderItem>();
        }
    }
}
