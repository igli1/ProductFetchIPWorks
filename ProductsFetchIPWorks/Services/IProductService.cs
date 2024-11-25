using ProductsFetchIPWorks.Dto;

namespace ProductsFetchIPWorks.Services;

public interface IProductService
{
    Task<ServiceResponse<IEnumerable<Product>>> GetAllProductsAsync();
}