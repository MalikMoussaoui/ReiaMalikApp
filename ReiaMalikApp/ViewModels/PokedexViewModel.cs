using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReiaMalikApp.Models;
using ReiaMalikApp.Services;

namespace ReiaMalikApp.ViewModels;

// L'héritage de ObservableObject est la clé du MVVM moderne
public partial class PokedexViewModel : ObservableObject
{
    private readonly PokeApiService _pokeApiService;

    // ObservableProperty génère automatiquement le code pour rafraîchir l'interface
    [ObservableProperty]
    private ObservableCollection<Pokemon> _pokemons;

    [ObservableProperty]
    private bool _isBusy;

    // Injection du service dans le constructeur
    public PokedexViewModel(PokeApiService pokeApiService)
    {
        _pokeApiService = pokeApiService;
        Pokemons = new ObservableCollection<Pokemon>();

        // On charge les données dès la création du ViewModel
        LoadPokemonsAsync();
    }

    [RelayCommand]
    public async Task LoadPokemonsAsync()
    {
        if (IsBusy) return;

        IsBusy = true; // Affiche l'indicateur de chargement
        try
        {
            var data = await _pokeApiService.GetPokemonsAsync();
            Pokemons.Clear();
            foreach (var pokemon in data)
            {
                Pokemons.Add(pokemon);
            }
        }
        finally
        {
            IsBusy = false; // Cache l'indicateur de chargement
        }
    }
}