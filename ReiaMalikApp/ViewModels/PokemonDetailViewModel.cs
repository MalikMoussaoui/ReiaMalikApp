using CommunityToolkit.Mvvm.ComponentModel;
using ReiaMalikApp.Models;

namespace ReiaMalikApp.ViewModels;

// Cette ligne magique récupère le paramètre "PokemonData" envoyé lors du clic
[QueryProperty(nameof(SelectedPokemon), "PokemonData")]
public partial class PokemonDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private Pokemon _selectedPokemon;
}