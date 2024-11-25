using System.Text.Json.Serialization;

namespace ProductsFetchIPWorks.Dto;

public class ProductResponse
{
    [JsonPropertyName("products")]
    public IEnumerable<Product> Products { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("skip")]
    public int Skip { get; set; }
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
}