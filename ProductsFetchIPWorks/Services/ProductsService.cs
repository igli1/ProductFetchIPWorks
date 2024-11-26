using Microsoft.Extensions.Options;
using nsoftware.IPWorks;
using ProductsFetchIPWorks.Config;
using ProductsFetchIPWorks.Dto;

namespace ProductsFetchIPWorks.Services;

public class ProductService : IProductService
{
    private readonly HTTP _httpClient;
    private ProductsApiConfig _productsApi;
    private readonly JSON jsonParser;
    public ProductService(IOptions<ProductsApiConfig> apiOptions)
    {
        _productsApi = apiOptions.Value;
        _httpClient = new HTTP
        {
            RuntimeLicense = "31504E4A415A3131323532343330574542545231413100495946545855464249444F51504B41450030303030303030300000353458463055395A553359580000#IPWORKS#EXPIRING_TRIAL#20241225"
        };
        jsonParser = new JSON
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
                
                //var productsResponse = JsonSerializer.Deserialize<ProductResponse>(apiResponse);
                jsonParser.BuildDOM = true;
                jsonParser.InputData = apiResponse;
                jsonParser.Parse();
                jsonParser.XPath = "$.products";
                var productsCount = jsonParser.XChildren.Count;
                for (int i = 0; i < productsCount; i++)
                {
                    var product = new Product();
                    
                    jsonParser.XPath = $"$.products[{i}].id";
                    if (int.TryParse(jsonParser.XText, out int productId))
                    {
                        product.Id = productId;
                    }

                    jsonParser.XPath = $"$.products[{i}].title";
                    product.Title = jsonParser.XText;

                    allProducts.Add(product);
                }
                
                jsonParser.XPath = "$.skip";
                int skip = int.TryParse(jsonParser.XText, out skip) ? skip : 0;

                jsonParser.XPath = "$.limit";
                int limit = int.TryParse(jsonParser.XText, out limit) ? limit : 0;

                jsonParser.XPath = "$.total";
                int total = int.TryParse(jsonParser.XText, out total) ? total : 0;

                if (skip + limit < total)
                {
                    currentPageUrl = string.Concat(_productsApi.ApiUrl, "products?skip=", skip + limit, "&limit=", limit);
                }
                else
                {
                    hasNext = false;
                }
                jsonParser.Reset();
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