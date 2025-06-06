using Microsoft.EntityFrameworkCore;

namespace EP.Order.Models
{
    /// <summary>
    /// Domain model for order items.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// The unique identifier of the order item.
        /// </summary>
        public int Id              { get; set; }

        /// <summary>
        /// The order id. Foreign key to the Order.
        /// </summary>
        public int OrderId         { get; set; }

        /// <summary>
        /// The id of the ordered product.
        /// </summary>
        public int ProductId       { get; set; }

        /// <summary>
        /// The quantity of the ordered item.
        /// </summary>
        public int Quantity        { get; set; }

        /// <summary>
        /// The unit price of the ordered product.
        /// </summary>
        [Precision(18, 2)]
        public decimal UnitPrice   { get; set; }

        // Navigations -------------------------

        public virtual Order? Order { get; set; }
    }
}
