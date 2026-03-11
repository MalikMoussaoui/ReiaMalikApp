using System.Net.Http.Json;
using ReiaMalikApp.Models;

namespace ReiaMalikApp.Services;

public class PokeApiService
{
    private readonly HttpClient _httpClient = new HttpClient();

    // 1. CHARGEMENT DE LA LISTE DE BASE
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
                int id = offset + 1;
                foreach (var item in response.Results)
                {
                    pokemons.Add(new Pokemon
                    {
                        Name = char.ToUpper(item.Name[0]) + item.Name.Substring(1),
                        ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png"
                    });
                    id++;
                }
            }
        }
        catch { }
        return pokemons;
    }

    // 2. CHARGEMENT DES DÉTAILS COMPLETS (Quand on clique sur un Pokémon)
    public async Task GetExtraDetailsAsync(Pokemon pokemon)
    {
        try
        {
            // A. Requête technique (Stats, Poids, Taille, Types)
            var urlDetails = $"https://pokeapi.co/api/v2/pokemon/{pokemon.Name.ToLower()}";
            var details = await _httpClient.GetFromJsonAsync<PokeApiDetail>(urlDetails);

            if (details != null)
            {
                // Types (on gère s'il y a un ou deux types)
                pokemon.Type = string.Join(" / ", details.Types.Select(t => t.Type.Name.ToUpper()));

                // Stats
                pokemon.HP = details.Stats[0].Base_Stat.ToString();
                pokemon.Attack = details.Stats[1].Base_Stat.ToString();
                pokemon.Defense = details.Stats[2].Base_Stat.ToString();

                // Conversion Poids (hectogrammes -> kg) et Taille (décimètres -> mètres)
                pokemon.Height = (details.Height / 10.0).ToString("0.0") + " m";
                pokemon.Weight = (details.Weight / 10.0).ToString("0.0") + " kg";

                // Talent principal
                if (details.Abilities.Count > 0)
                {
                    pokemon.Ability = char.ToUpper(details.Abilities[0].Ability.Name[0]) + details.Abilities[0].Ability.Name.Substring(1);
                }
            }

            // B. Requête pour le Français (Description et Catégorie)
            var urlSpecies = $"https://pokeapi.co/api/v2/pokemon-species/{pokemon.Name.ToLower()}";
            var species = await _httpClient.GetFromJsonAsync<PokeApiSpecies>(urlSpecies);

            if (species != null)
            {
                // Cherche la description en FR
                var frFlavor = species.FlavorTextEntries.FirstOrDefault(f => f.Language.Name == "fr");
                if (frFlavor != null)
                {
                    // On nettoie les sauts de lignes bizarres de l'API
                    pokemon.Description = frFlavor.FlavorText.Replace("\n", " ").Replace("\f", " ");
                }
                else
                {
                    pokemon.Description = "Description française non disponible.";
                }

                // Cherche la catégorie en FR (Ex: "Pokémon Graine")
                var frGenus = species.Genera.FirstOrDefault(g => g.Language.Name == "fr");
                if (frGenus != null)
                {
                    pokemon.Category = frGenus.Genus;
                }
            }
        }
        catch
        {
            pokemon.Description = "Erreur de connexion lors du chargement des détails.";
        }
    }
}