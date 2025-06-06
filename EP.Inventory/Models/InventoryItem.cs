namespace EP.Inventory.Models
{
    /// <summary>
    /// Domain model for inventory items.
    /// </summary>
    public class InventoryItem
    {
        /// <summary>
        /// The unique identifier of the inventory item.
        /// </summary>
        public int Id             { get; set; }

        /// <summary>
        /// The id of the product.
        /// </summary>
        public int ProductId      { get; set; }

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The total quantity of the product.
        /// </summary>
        public int Quantity       { get; set; }
    }
}
