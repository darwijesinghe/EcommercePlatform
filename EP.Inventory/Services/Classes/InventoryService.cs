using EP.Inventory.Models;
using EP.Inventory.Services.Interfaces;
using Mapster;
using SharedLibrary.DTOs;
using SharedLibrary.Helpers.Extensions;
using SharedLibrary.Response;
using SharedLibrary.Services.Interfaces;

namespace EP.Inventory.Services.Classes
{
    /// <summary>
    /// Provides an implementation of <see cref="IInventoryService"/> that manages the inventory data functionalities.
    /// </summary>
    public class InventoryService : IInventoryService
    {
        // Repositories
        private readonly IGenericRepository<InventoryItem> _repository;

        public InventoryService(IGenericRepository<InventoryItem> repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<Result<IEnumerable<InventoryItemDto>>> GetInventoryAsync()
        {
            try
            {
                // gets all inventory data
                var entities = await _repository.GetAllAsync();
                if (entities.IsNullOrEmpty())
                    return new Result<IEnumerable<InventoryItemDto>> { Message = "No data found." };

                // mapping data
                var data = entities.Adapt<IEnumerable<InventoryItemDto>>();

                return new Result<IEnumerable<InventoryItemDto>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<InventoryItemDto>> { Message = ex.Message };
            }
        }
    }
}
