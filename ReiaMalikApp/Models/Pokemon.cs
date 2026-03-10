using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ReiaMalikApp.Models;

// Le modèle principal utilisé par l'interface
public class Pokemon
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public string Type { get; set; } = "Inconnu";
    public string HP { get; set; } = "??";
    public string Attack { get; set; } = "??";
    public string Defense { get; set; } = "??";
}

// --- CLASSES POUR L'API (Celles qui manquaient !) ---

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

public class PokeApiDetail
{
    [JsonPropertyName("types")]
    public List<PokeApiTypeSlot> Types { get; set; }

    [JsonPropertyName("stats")]
    public List<PokeApiStatSlot> Stats { get; set; }
}

public class PokeApiTypeSlot { [JsonPropertyName("type")] public PokeApiType Type { get; set; } }
public class PokeApiType { [JsonPropertyName("name")] public string Name { get; set; } }
public class PokeApiStatSlot { [JsonPropertyName("base_stat")] public int Base_Stat { get; set; } }