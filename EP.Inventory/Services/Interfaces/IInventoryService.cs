using SharedLibrary.DTOs;
using SharedLibrary.Response;

namespace EP.Inventory.Services.Interfaces
{
    /// <summary>
    /// Defines contracts of inventory related functionalities.
    /// </summary>
    public interface IInventoryService
    {
        /// <summary>
        /// Retrieves all the inventory data.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{IEnumerable{InventoryItemDto}}"/> containing a list of inventory data.
        /// </returns>
        Task<Result<IEnumerable<InventoryItemDto>>> GetInventoryAsync();

    }
}
