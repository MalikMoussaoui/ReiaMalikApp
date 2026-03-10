using System.Net.Http.Json;
using ReiaMalikApp.Models;

namespace ReiaMalikApp.Services;

public class PokeApiService
{
    private readonly HttpClient _httpClient;

    public PokeApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Pokemon>> GetPokemonsByGenerationAsync(int generation)
    {
        var pokemons = new List<Pokemon>();

        // On définit le nombre de Pokémon et le point de départ selon la génération choisie
        int limit = 151;
        int offset = 0;

        switch (generation)
        {
            case 1: limit = 151; offset = 0; break;
            case 2: limit = 100; offset = 151; break;
            case 3: limit = 135; offset = 251; break;
            case 4: limit = 107; offset = 386; break;
            case 5: limit = 156; offset = 493; break;
            case 6: limit = 72; offset = 649; break;
            case 7: limit = 88; offset = 721; break;
            case 8: limit = 96; offset = 809; break;
            case 9: limit = 112; offset = 905; break; // Génération 9 (Paldea)
        }

        try
        {
            // On utilise la bonne vieille API mondiale qui ne plante JAMAIS
            var url = $"https://pokeapi.co/api/v2/pokemon?limit={limit}&offset={offset}";
            var response = await _httpClient.GetFromJsonAsync<PokeApiResponse>(url);

            if (response != null && response.Results != null)
            {
                // L'ID du premier Pokémon de cette génération
                int id = offset + 1;

                foreach (var item in response.Results)
                {
                    pokemons.Add(new Pokemon
                    {
                        // Majuscule au nom
                        Name = char.ToUpper(item.Name[0]) + item.Name.Substring(1),
                        // Récupération de l'image haute qualité
                        ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png",
                        Description = $"Pokédex N°{id}\nGénération {generation}"
                    });
                    id++;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
        }

        return pokemons;
    }
}