using System.Text.Json.Serialization;

namespace ReiaMalikApp.Models;

public class Pokemon
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}

public class PokeApiResponse
{
    [JsonPropertyName("results")]
    public List<PokeApiResult> Results { get; set; } = new();
}

public class PokeApiResult
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}