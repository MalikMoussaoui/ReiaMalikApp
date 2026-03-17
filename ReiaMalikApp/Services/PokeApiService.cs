using System.Net.Http.Json;
using ReiaMalikApp.Models;

namespace ReiaMalikApp.Services;

public class PokeApiService
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<List<Pokemon>> GetPokemonsByGenerationAsync(int generation)
    {
        var pokemons = new List<Pokemon>();
        int limit = 151; int offset = 0;

        switch (generation)
        {
            case 1: limit = 151; offset = 0; break;
            case 2: limit = 100; offset = 151; break;
            case 3: limit = 135; offset = 251; break;
            case 4: limit = 107; offset = 386; break;
            default: limit = 151; offset = 0; break;
        }

        try
        {
            var url = $"https://pokeapi.co/api/v2/pokemon?limit={limit}&offset={offset}";
            var response = await _httpClient.GetFromJsonAsync<PokeApiResponse>(url);
            if (response?.Results != null)
            {
                var tasks = new List<Task<Pokemon>>();
                int id = offset + 1;
                foreach (var item in response.Results)
                {
                    tasks.Add(FetchPokemonBasicInfoAsync(item.Name, item.Url, id, generation));
                    id++;
                }
                var results = await Task.WhenAll(tasks);
                pokemons.AddRange(results);
            }
        }
        catch { }
        return pokemons;
    }

    private async Task<Pokemon> FetchPokemonBasicInfoAsync(string name, string url, int id, int generation)
    {
        var p = new Pokemon
        {
            Name = char.ToUpper(name[0]) + name.Substring(1),
            ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png",
            Generation = generation
        };

        try
        {
            var detail = await _httpClient.GetFromJsonAsync<PokeApiDetail>(url);
            if (detail != null)
            {
                if (detail.Types.Count > 0) p.Type1 = detail.Types[0].Type.Name.ToUpper();
                if (detail.Types.Count > 1) p.Type2 = detail.Types[1].Type.Name.ToUpper();
            }
        }
        catch { p.Type1 = "???"; }
        return p;
    }

    public async Task GetExtraDetailsAsync(Pokemon pokemon)
    {
        if (pokemon.Generation == 5) return;

        try
        {
            var urlDetails = $"https://pokeapi.co/api/v2/pokemon/{pokemon.Name.ToLower()}";
            var details = await _httpClient.GetFromJsonAsync<PokeApiDetail>(urlDetails);
            if (details != null)
            {
                if (details.Types.Count > 0) pokemon.Type1 = details.Types[0].Type.Name.ToUpper();
                if (details.Types.Count > 1) pokemon.Type2 = details.Types[1].Type.Name.ToUpper();

                pokemon.HP = details.Stats[0].Base_Stat.ToString();
                pokemon.Attack = details.Stats[1].Base_Stat.ToString();
                pokemon.Defense = details.Stats[2].Base_Stat.ToString();
                pokemon.Height = (details.Height / 10.0).ToString("0.0") + " m";
                pokemon.Weight = (details.Weight / 10.0).ToString("0.0") + " kg";
                if (details.Abilities.Count > 0)
                {
                    pokemon.Ability = char.ToUpper(details.Abilities[0].Ability.Name[0]) + details.Abilities[0].Ability.Name.Substring(1);
                }
            }

            var urlSpecies = $"https://pokeapi.co/api/v2/pokemon-species/{pokemon.Name.ToLower()}";
            var species = await _httpClient.GetFromJsonAsync<PokeApiSpecies>(urlSpecies);
            if (species != null)
            {
                var frFlavor = species.FlavorTextEntries.FirstOrDefault(f => f.Language.Name == "fr");
                pokemon.Description = frFlavor != null ? frFlavor.FlavorText.Replace("\n", " ").Replace("\f", " ") : "Description non disponible.";
                var frGenus = species.Genera.FirstOrDefault(g => g.Language.Name == "fr");
                if (frGenus != null) pokemon.Category = frGenus.Genus;
            }
        }
        catch { pokemon.Description = "Erreur de connexion."; }
    }
}
