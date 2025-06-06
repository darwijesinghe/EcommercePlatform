using SharedLibrary.DTOs;
using SharedLibrary.Response;

namespace EP.Order.Services.Interfaces
{
    /// <summary>
    /// Defines contracts of order related functionalities.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{IEnumerable{OrderDto}}"/> containing a list of orders.
        /// </returns>
        Task<Result<IEnumerable<OrderDto>>> GetAllOrdersAsync();

        /// <summary>
        /// Retrieves order by its PK.
        /// </summary>
        /// <param name="id">The PK of the order.</param>
        /// <returns>
        /// A <see cref="Result{OrderDto}"/> containing a data of the order.
        /// </returns>
        Task<Result<OrderDto>> GetOrderAsync(int id);

        /// <summary>
        /// Creates a new order to the database and publishing order data to RabbitMQ message broker.
        /// </summary>
        /// <param name="data">The order data to be stored.</param>
        /// <returns>
        /// A <see cref="Result"/> that contains the order creation process result.
        /// </returns>
        Task<Result> CreateOrderAsync(OrderDto data);
    }
}
