using CommunityToolkit.Mvvm.ComponentModel;
using ReiaMalikApp.Models;
using ReiaMalikApp.Services;

namespace ReiaMalikApp.ViewModels;

[QueryProperty(nameof(SelectedPokemon), "Pokemon")]
public partial class PokemonDetailViewModel : ObservableObject
{
    private readonly PokeApiService _apiService;

    [ObservableProperty]
    private Pokemon _selectedPokemon;

    public PokemonDetailViewModel(PokeApiService apiService)
    {
        _apiService = apiService;
    }

    async partial void OnSelectedPokemonChanged(Pokemon value)
    {
        if (value != null)
        {
            await _apiService.GetExtraDetailsAsync(value);

            OnPropertyChanged(nameof(SelectedPokemon));
        }
    }
}