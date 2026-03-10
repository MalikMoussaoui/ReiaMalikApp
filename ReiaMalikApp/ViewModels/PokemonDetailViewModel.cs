using CommunityToolkit.Mvvm.ComponentModel;
using ReiaMalikApp.Models;

namespace ReiaMalikApp.ViewModels;

// [QueryProperty] permet de récupérer l'objet envoyé lors de la navigation
[QueryProperty(nameof(SelectedPokemon), "PokemonData")]
public partial class PokemonDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private Pokemon _selectedPokemon;
}