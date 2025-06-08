using EP.Order.Models;
using EP.Order.Services.Interfaces;
using Mapster;
using SharedLibrary.DTOs;
using SharedLibrary.Helpers.Extensions;
using SharedLibrary.Response;
using SharedLibrary.Services.Interfaces;

namespace EP.Order.Services.Classes
{
    /// <summary>
    /// Provides an implementation of <see cref="IOrderService"/> that manages the order data functionalities.
    /// </summary>
    public class OrderService : IOrderService
    {
        // Repositories
        private readonly IGenericRepository<Models.Order> _repository;

        // Services
        private readonly IMessagePublisher                _messagePublisher;

        public OrderService(IGenericRepository<Models.Order> repository, IMessagePublisher messagePublisher)
        {
            _repository       = repository;
            _messagePublisher = messagePublisher;
        }

        /// <inheritdoc/>
        public async Task<Result> CreateOrderAsync(OrderDto data)
        {
            try
            {
                // validations
                if (data is null)
                    return new Result { Message = "Required data not found." };

                // TODO: 1. Validate products before placing the order by calling the product service
                //       2. Validate the inventory stock before placing the order by calling the inventory service 

                // data mapping
                var order = data.Adapt<Models.Order>();

                // operation
                await _repository.AddAsync(order);
                await _repository.SaveChangesAsync();

                // maps to a DTO for messaging
                var message = order.Adapt<OrderDto>();

                // publishing the product data to message broker
                _messagePublisher.Publish(message);

                return new Result { Success = true };
            }
            catch (Exception ex)
            {
                return new Result { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            try
            {
                // gets all orders
                var entities = await _repository.GetAllAsync();
                if (entities.IsNullOrEmpty())
                    return new Result<IEnumerable<OrderDto>> { Message = "No data found." };

                // mapping data
                var data = entities.Adapt<IEnumerable<OrderDto>>();

                return new Result<IEnumerable<OrderDto>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<OrderDto>> { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<OrderDto>> GetOrderAsync(int id)
        {
            try
            {
                // validations
                if (id < 0)
                    return new Result<OrderDto> { Message = "Product ID is not valid." };

                // gets a order by its PK
                var entity = await _repository.GetByConditionAsync(x => x.Id == id);
                if (entity is null)
                    return new Result<OrderDto> { Message = "No data found." };

                // mapping data
                var data = entity.Adapt<OrderDto>();

                return new Result<OrderDto> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<OrderDto> { Message = ex.Message };
            }
        }
    }
}
