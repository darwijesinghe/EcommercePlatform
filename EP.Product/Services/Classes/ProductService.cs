using EP.Product.Services.Interfaces;
using Mapster;
using SharedLibrary.DTOs;
using SharedLibrary.Helpers.Extensions;
using SharedLibrary.Response;
using SharedLibrary.Services.Interfaces;

namespace EP.Product.Services.Classes
{
    /// <summary>
    /// Provides an implementation of <see cref="IProductService"/> that manages the product data functionalities.
    /// </summary>
    public class ProductService : IProductService
    {
        // Repositories
        private readonly IGenericRepository<Models.Product> _repository;

        // Services
        private readonly IMessagePublisher                  _messagePublisher;

        public ProductService(IGenericRepository<Models.Product> repository, IMessagePublisher messagePublisher)
        {
            _repository       = repository;
            _messagePublisher = messagePublisher;
        }

        /// <inheritdoc/>
        public async Task<Result> AddProductAsync(ProductDto data)
        {
            try
            {
                // validations
                if (data is null)
                    return new Result { Message = "Required data not found." };

                // checks the product existence
                var isExist = await _repository.IsExist(x => x.Id == data.Id);
                if(isExist)
                    return new Result { Message = "This product is already exist." };

                // data mapping
                var product = data.Adapt<Models.Product>();

                // operation
                await _repository.AddAsync(product);
                await _repository.SaveChangesAsync();

                // maps to a DTO for messaging
                var message = product.Adapt<ProductDto>();

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
        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                // gets all products
                var entities = await _repository.GetAllAsync();
                if (entities.IsNullOrEmpty())
                    return new Result<IEnumerable<ProductDto>> { Message = "No data found." };

                // data mapping
                var data = entities.Adapt<IEnumerable<ProductDto>>();

                return new Result<IEnumerable<ProductDto>> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<ProductDto>> { Message = ex.Message };
            }
        }

        /// <inheritdoc/>
        public async Task<Result<ProductDto>> GetProductAsync(int id)
        {
            try
            {
                // validations
                if (id < 0)
                    return new Result<ProductDto> { Message = "Product ID is not valid." };

                // gets a product by its PK
                var entity = await _repository.GetByConditionAsync(x => x.Id == id);
                if (entity is null)
                    return new Result<ProductDto> { Message = "No data found." };

                // data mapping
                var data = entity.Adapt<ProductDto>();

                return new Result<ProductDto> { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new Result<ProductDto> { Message = ex.Message };
            }
        }
    }
}
