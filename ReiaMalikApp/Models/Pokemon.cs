using System.Text.Json.Serialization;

namespace ReiaMalikApp.Models;

public class Pokemon
{
    public static List<Pokemon> GenerationS = new();

    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; } = "Préparation des données...";
    public string Type1 { get; set; } = "...";
    public string Type2 { get; set; } = "";
    public int Generation { get; set; } = 1;
    public string HP { get; set; } = "0";
    public string Attack { get; set; } = "0";
    public string Defense { get; set; } = "0";
    public string Height { get; set; } = "...";
    public string Weight { get; set; } = "...";
    public string Category { get; set; } = "...";
    public string Ability { get; set; } = "...";

    public bool HasType2 => !string.IsNullOrWhiteSpace(Type2);

    public Color Type1Color => GetTypeColor(Type1);
    public Color Type2Color => GetTypeColor(Type2);

    public Color GenerationColor => Generation switch
    {
        1 => Color.FromArgb("#2C3E50"),
        2 => Color.FromArgb("#1E8449"),
        3 => Color.FromArgb("#A93226"),
        4 => Color.FromArgb("#6C3483"),
        5 => Color.FromArgb("#B9770E"),
        _ => Color.FromArgb("#2C3E50")
    };

    private Color GetTypeColor(string type)
    {
        if (string.IsNullOrWhiteSpace(type)) return Color.FromArgb("#E3350D");
        return type.ToUpper() switch
        {
            "GRASS" or "PLANTE" => Color.FromArgb("#7AC74C"),
            "FIRE" or "FEU" => Color.FromArgb("#EE8130"),
            "WATER" or "EAU" => Color.FromArgb("#6390F0"),
            "BUG" or "INSECTE" => Color.FromArgb("#A6B91A"),
            "NORMAL" => Color.FromArgb("#A8A77A"),
            "POISON" => Color.FromArgb("#A33EA1"),
            "ELECTRIC" or "ELECTRIK" => Color.FromArgb("#F7D02C"),
            "GROUND" or "SOL" => Color.FromArgb("#E2BF65"),
            "FAIRY" or "FÉE" => Color.FromArgb("#D685AD"),
            "FIGHTING" or "COMBAT" => Color.FromArgb("#C22E28"),
            "PSYCHIC" or "PSY" => Color.FromArgb("#F95587"),
            "ROCK" or "ROCHE" => Color.FromArgb("#B6A136"),
            "GHOST" or "SPECTRE" => Color.FromArgb("#735797"),
            "ICE" or "GLACE" => Color.FromArgb("#96D9D6"),
            "DRAGON" => Color.FromArgb("#6F35FC"),
            "DARK" or "TÉNÈBRES" or "TENEBRES" => Color.FromArgb("#705746"),
            "STEEL" or "ACIER" => Color.FromArgb("#B7B7CE"),
            "FLYING" or "VOL" => Color.FromArgb("#A98FF3"),
            _ => Color.FromArgb("#E3350D")
        };
    }
}

public class PokeApiResponse { [JsonPropertyName("results")] public List<PokeApiResult> Results { get; set; } = new(); }
public class PokeApiResult { [JsonPropertyName("name")] public string Name { get; set; } [JsonPropertyName("url")] public string Url { get; set; } }
public class PokeApiDetail { [JsonPropertyName("types")] public List<PokeApiTypeSlot> Types { get; set; } [JsonPropertyName("stats")] public List<PokeApiStatSlot> Stats { get; set; } [JsonPropertyName("height")] public int Height { get; set; } [JsonPropertyName("weight")] public int Weight { get; set; } [JsonPropertyName("abilities")] public List<PokeApiAbilitySlot> Abilities { get; set; } }
public class PokeApiTypeSlot { [JsonPropertyName("type")] public PokeApiType Type { get; set; } }
public class PokeApiType { [JsonPropertyName("name")] public string Name { get; set; } }
public class PokeApiStatSlot { [JsonPropertyName("base_stat")] public int Base_Stat { get; set; } }
public class PokeApiAbilitySlot { [JsonPropertyName("ability")] public PokeApiAbility Ability { get; set; } }
public class PokeApiAbility { [JsonPropertyName("name")] public string Name { get; set; } }
public class PokeApiSpecies { [JsonPropertyName("flavor_text_entries")] public List<PokeApiFlavorText> FlavorTextEntries { get; set; } [JsonPropertyName("genera")] public List<PokeApiGenus> Genera { get; set; } }
public class PokeApiFlavorText { [JsonPropertyName("flavor_text")] public string FlavorText { get; set; } [JsonPropertyName("language")] public PokeApiLanguage Language { get; set; } }
public class PokeApiGenus { [JsonPropertyName("genus")] public string Genus { get; set; } [JsonPropertyName("language")] public PokeApiLanguage Language { get; set; } }
public class PokeApiLanguage { [JsonPropertyName("name")] public string Name { get; set; } }
