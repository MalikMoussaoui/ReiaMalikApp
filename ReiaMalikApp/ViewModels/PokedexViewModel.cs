using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReiaMalikApp.Models;
using ReiaMalikApp.Services;

namespace ReiaMalikApp.ViewModels;

public partial class PokedexViewModel : ObservableObject
{
    private readonly PokeApiService _apiService;
    private List<Pokemon> _allPokemons = new();

    [ObservableProperty]
    private ObservableCollection<Pokemon> _pokemons;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _loadingText = "Préparation du Pokédex...";

    private readonly string[] _funnyPhrases = {
        "Attrapage des Pokémon sauvages...", "Réveil de Ronflex...", "Nettoyage des Poké Balls..."
    };

    public PokedexViewModel(PokeApiService apiService)
    {
        _apiService = apiService;
        Pokemons = new ObservableCollection<Pokemon>();
        _ = LoadPokemonsAsync(1);
    }

    [RelayCommand]
    public async Task LoadPokemonsAsync(int generation = 1)
    {
        if (IsLoading) return;

        IsLoading = true;

        try
        {
            Pokemons.Clear();
            _allPokemons.Clear();

            if (generation == 5)
            {
                LoadingText = "Ouverture du Coffre S...";
                await Task.Delay(800);
                if (Pokemon.GenerationS != null) _allPokemons = new List<Pokemon>(Pokemon.GenerationS);
            }
            else if (generation == 6)
            {
                LoadingText = "Connexion au PC de Léo...";
                await Task.Delay(800);
                if (Pokemon.Captured != null) _allPokemons = new List<Pokemon>(Pokemon.Captured);
            }
            else
            {
                var cts = new CancellationTokenSource();
                _ = CycleLoadingTextAsync(cts.Token);
                var results = await _apiService.GetPokemonsByGenerationAsync(generation);
                if (results != null) _allPokemons = results;
                cts.Cancel();
            }

            foreach (var pokemon in _allPokemons)
            {
                Pokemons.Add(pokemon);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public void SearchPokemon(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            Pokemons = new ObservableCollection<Pokemon>(_allPokemons);
        }
        else
        {
            var filtered = _allPokemons.Where(p => p.Name.ToLower().Contains(query.ToLower())).ToList();
            Pokemons = new ObservableCollection<Pokemon>(filtered);
        }
    }

    private async Task CycleLoadingTextAsync(CancellationToken token)
    {
        int index = 0;
        while (!token.IsCancellationRequested)
        {
            LoadingText = _funnyPhrases[index % _funnyPhrases.Length];
            index++;
            try { await Task.Delay(1500, token); } catch { }
        }
    }
}