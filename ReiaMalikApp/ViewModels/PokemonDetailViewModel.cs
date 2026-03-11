using CommunityToolkit.Mvvm.ComponentModel;
using ReiaMalikApp.Models;
using ReiaMalikApp.Services;

namespace ReiaMalikApp.ViewModels;

// Permet de récupérer le paramètre envoyé par la page précédente
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

    // Cette méthode "magique" se déclenche toute seule quand le SelectedPokemon est assigné
    async partial void OnSelectedPokemonChanged(Pokemon value)
    {
        if (value != null)
        {
            // On demande à l'API d'aller chercher le reste des infos
            await _apiService.GetExtraDetailsAsync(value);

            // On force l'interface graphique à se rafraîchir avec les nouvelles données
            OnPropertyChanged(nameof(SelectedPokemon));
        }
    }
}