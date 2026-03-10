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
            default: limit = 151; offset = 0; break;
        }

        try
        {
            var url = $"https://pokeapi.co/api/v2/pokemon?limit={limit}&offset={offset}";
            var response = await _httpClient.GetFromJsonAsync<PokeApiResponse>(url);
            if (response?.Results != null)
            {
                int id = offset + 1;
                foreach (var item in response.Results)
                {
                    pokemons.Add(new Pokemon
                    {
                        Name = char.ToUpper(item.Name[0]) + item.Name.Substring(1),
                        ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png",
                        Description = $"Pokémon originaire de la génération {generation}.",
                        // On stocke l'ID temporairement dans la description pour le retrouver
                        HP = id.ToString()
                    });
                    id++;
                }
            }
        }
        catch { }
        return pokemons;
    }

    // NOUVELLE MÉTHODE : Va chercher les vraies stats et types
    public async Task GetExtraDetailsAsync(Pokemon pokemon)
    {
        try
        {
            var url = $"https://pokeapi.co/api/v2/pokemon/{pokemon.Name.ToLower()}";
            var details = await _httpClient.GetFromJsonAsync<PokeApiDetail>(url);
            if (details != null)
            {
                pokemon.Type = details.Types[0].Type.Name.ToUpper();
                pokemon.HP = details.Stats[0].Base_Stat.ToString();
                pokemon.Attack = details.Stats[1].Base_Stat.ToString();
                pokemon.Defense = details.Stats[2].Base_Stat.ToString();
            }
        }
        catch { }
    }
}