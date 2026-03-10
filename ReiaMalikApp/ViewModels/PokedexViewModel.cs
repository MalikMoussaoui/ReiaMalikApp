using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReiaMalikApp.Models;
using ReiaMalikApp.Services;

namespace ReiaMalikApp.ViewModels;

public partial class PokedexViewModel : ObservableObject
{
    private readonly PokeApiService _pokeApiService;

    [ObservableProperty]
    private ObservableCollection<Pokemon> _pokemons;

    [ObservableProperty]
    private bool _isBusy;

    // --- Gestion des 9 Générations ---
    [ObservableProperty]
    private int _selectedGeneration;

    public List<int> Generations { get; } = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // --- Correction du bug de clic (Sélection robuste) ---
    [ObservableProperty]
    private Pokemon _selectedPokemonItem;

    public PokedexViewModel(PokeApiService pokeApiService)
    {
        _pokeApiService = pokeApiService;
        Pokemons = new ObservableCollection<Pokemon>();

        // On assigne discrètement le "1" à la variable cachée pour éviter un double-chargement
        _selectedGeneration = 1;

        // On force le chargement des Pokémon immédiatement au lancement
        LoadPokemonsAsync();
    }

    // Magie du MVVM : Cette méthode s'exécute toute seule quand on change de génération !
    async partial void OnSelectedGenerationChanged(int value)
    {
        await LoadPokemonsAsync();
    }

    // Cette méthode s'exécute quand on clique sur un Pokémon (corrige ton bug d'écran vide)
    async partial void OnSelectedPokemonItemChanged(Pokemon value)
    {
        if (value != null)
        {
            var navigationParameter = new Dictionary<string, object> { { "PokemonData", value } };
            await Shell.Current.GoToAsync(nameof(Views.PokemonDetailPage), navigationParameter);

            // On remet à null pour pouvoir recliquer sur le même plus tard
            SelectedPokemonItem = null;
        }
    }

    [RelayCommand]
    public async Task LoadPokemonsAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        try
        {
            // On charge la génération sélectionnée !
            var data = await _pokeApiService.GetPokemonsByGenerationAsync(SelectedGeneration);
            Pokemons.Clear();
            foreach (var pokemon in data)
            {
                Pokemons.Add(pokemon);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}