using CommunityToolkit.Mvvm.ComponentModel;
using ReiaMalikApp.Models;
using ReiaMalikApp.Services;

namespace ReiaMalikApp.ViewModels;

[QueryProperty(nameof(SelectedPokemon), "PokemonData")]
public partial class PokemonDetailViewModel : ObservableObject
{
    private readonly PokeApiService _apiService;

    [ObservableProperty]
    private Pokemon _selectedPokemon;

    public PokemonDetailViewModel(PokeApiService apiService)
    {
        _apiService = apiService;
    }

    // Dès que le Pokémon arrive, on lance la recherche des stats
    async partial void OnSelectedPokemonChanged(Pokemon value)
    {
        if (value != null && value.Type == "Inconnu")
        {
            await _apiService.GetExtraDetailsAsync(value);
            // On force la mise à jour de l'interface
            OnPropertyChanged(nameof(SelectedPokemon));
        }
    }
}