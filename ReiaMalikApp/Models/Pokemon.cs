using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ReiaMalikApp.Models;

// 1. NOTRE OBJET POKÉMON (Celui qu'on affiche à l'écran)
public class Pokemon
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }

    // On met des valeurs par défaut pour voir le chargement
    public string Description { get; set; } = "Recherche des données dans le Pokédex...";
    public string Type { get; set; } = "...";
    public string HP { get; set; } = "0";
    public string Attack { get; set; } = "0";
    public string Defense { get; set; } = "0";

    // Nouvelles infos pour le design officiel
    public string Height { get; set; } = "...";
    public string Weight { get; set; } = "...";
    public string Category { get; set; } = "...";
    public string Ability { get; set; } = "...";
}

// --------------------------------------------------------
// 2. CLASSES TECHNIQUES (Pour traduire le JSON de l'API)
// --------------------------------------------------------

// Pour la liste de base
public class PokeApiResponse { [JsonPropertyName("results")] public List<PokeApiResult> Results { get; set; } = new(); }
public class PokeApiResult { [JsonPropertyName("name")] public string Name { get; set; } [JsonPropertyName("url")] public string Url { get; set; } }

// Pour les détails techniques (Stats, Types, Poids...)
public class PokeApiDetail
{
    [JsonPropertyName("types")] public List<PokeApiTypeSlot> Types { get; set; }
    [JsonPropertyName("stats")] public List<PokeApiStatSlot> Stats { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("weight")] public int Weight { get; set; }
    [JsonPropertyName("abilities")] public List<PokeApiAbilitySlot> Abilities { get; set; }
}
public class PokeApiTypeSlot { [JsonPropertyName("type")] public PokeApiType Type { get; set; } }
public class PokeApiType { [JsonPropertyName("name")] public string Name { get; set; } }
public class PokeApiStatSlot { [JsonPropertyName("base_stat")] public int Base_Stat { get; set; } }
public class PokeApiAbilitySlot { [JsonPropertyName("ability")] public PokeApiAbility Ability { get; set; } }
public class PokeApiAbility { [JsonPropertyName("name")] public string Name { get; set; } }

// Pour le texte et la catégorie (Species)
public class PokeApiSpecies
{
    [JsonPropertyName("flavor_text_entries")] public List<PokeApiFlavorText> FlavorTextEntries { get; set; }
    [JsonPropertyName("genera")] public List<PokeApiGenus> Genera { get; set; }
}
public class PokeApiFlavorText { [JsonPropertyName("flavor_text")] public string FlavorText { get; set; } [JsonPropertyName("language")] public PokeApiLanguage Language { get; set; } }
public class PokeApiGenus { [JsonPropertyName("genus")] public string Genus { get; set; } [JsonPropertyName("language")] public PokeApiLanguage Language { get; set; } }
public class PokeApiLanguage { [JsonPropertyName("name")] public string Name { get; set; } }