using System.Text.Json;
using Microsoft.Extensions.Options;
using nsoftware.IPWorks;
using ProductsFetchIPWorks.Config;
using ProductsFetchIPWorks.Dto;

namespace ProductsFetchIPWorks.Services;

public class ProductService : IProductService
{
    private readonly HTTP _httpClient;
    private ProductsApiConfig _productsApi;
    
    public ProductService(IOptions<ProductsApiConfig> apiOptions)
    {
        _productsApi = apiOptions.Value;
        _httpClient = new HTTP
        {
            RuntimeLicense = "31504E4A415A3131323532343330574542545231413100495946545855464249444F51504B41450030303030303030300000353458463055395A553359580000#IPWORKS#EXPIRING_TRIAL#20241225"
        };
    }
    public async Task<ServiceResponse<IEnumerable<Product>>> GetAllProductsAsync()
    {
        var allProducts = new List<Product>();
        var response = new ServiceResponse<IEnumerable<Product>>();
        try
        {
            string currentPageUrl = string.Concat(_productsApi.ApiUrl,"products");
            bool hasNext = true;

            while (hasNext)
            {
                _httpClient.Get(currentPageUrl);
                
                string apiResponse = _httpClient.TransferredData;
                
                var productsResponse = JsonSerializer.Deserialize<ProductResponse>(apiResponse);

                if (productsResponse?.Products != null)
                {
                    allProducts.AddRange(productsResponse.Products);
                }
                
                if (productsResponse?.Skip + productsResponse?.Limit < productsResponse?.Total)
                {
                    currentPageUrl = string.Concat(_productsApi.ApiUrl,"products?skip=", productsResponse.Skip + productsResponse.Limit, "&limit=",productsResponse.Limit );
                }
                else
                {
                    hasNext = false;
                }
            }
            response.Status = true;
            response.Data = allProducts;
            return response;
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.Message = ex.Message;
            response.Data = new List<Product>();
            return response;
        }
    }

}