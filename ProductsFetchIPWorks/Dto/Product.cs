using System.Text.Json.Serialization;

namespace ProductsFetchIPWorks.Dto;

public class Product
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
}