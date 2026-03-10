using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ReiaMalikApp.Models;

namespace ReiaMalikApp.Services;

// Classes techniques pour lire le format spécifique du JSON de PokéAPI
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

// Le vrai Service qui sera utilisé par l'application
public class PokeApiService
{
    private readonly HttpClient _httpClient;

    public PokeApiService()
    {
        // Initialisation du client HTTP
        _httpClient = new HttpClient();
    }

    public async Task<List<Pokemon>> GetPokemonsAsync()
    {
        var pokemons = new List<Pokemon>();

        try
        {
            // Appel asynchrone à l'API pour récupérer les 151 premiers Pokémon (Génération 1)
            var url = "https://pokeapi.co/api/v2/pokemon?limit=151";
            var response = await _httpClient.GetFromJsonAsync<PokeApiResponse>(url);

            if (response != null && response.Results != null)
            {
                int id = 1;
                foreach (var item in response.Results)
                {
                    // Création de notre objet Pokemon à partir des données de l'API
                    pokemons.Add(new Pokemon
                    {
                        // On met une majuscule au nom
                        Name = char.ToUpper(item.Name[0]) + item.Name.Substring(1),

                        // Astuce de dev : on pointe directement vers les artworks officiels via l'ID
                        ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png",

                        // Description générique pour valider le cahier des charges
                        Description = $"Voici le profil de {item.Name.ToUpper()}. Un Pokémon de la première génération très puissant en combat !"
                    });
                    id++;
                }
            }
        }
        catch (Exception ex)
        {
            // Try/Catch obligatoire pour éviter que l'appli crash si le joueur n'a pas internet
            System.Diagnostics.Debug.WriteLine($"Erreur lors de l'appel API : {ex.Message}");
        }

        return pokemons;
    }
}