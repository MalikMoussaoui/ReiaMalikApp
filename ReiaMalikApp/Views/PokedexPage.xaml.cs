using ReiaMalikApp.Models;
using ReiaMalikApp.ViewModels;

namespace ReiaMalikApp.Views;

public partial class PokedexPage : ContentPage
{
    private readonly PokedexViewModel _viewModel;
    private bool _isAnimating = false; // <-- LA SÉCURITÉ ANTI-CRASH EST ICI !

    public PokedexPage(PokedexViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // 1. On empêche de lancer 100 animations en même temps
        if (!_isAnimating)
        {
            AnimatePokeball();
        }

        // 2. Si on était sur la Génération S, on actualise pour voir le nouveau Pokémon !
        if (GenerationPicker.SelectedIndex == 4)
        {
            _ = _viewModel.LoadPokemonsAsync(5);
        }
    }

    private async void AnimatePokeball()
    {
        _isAnimating = true; // On verrouille l'animation
        while (true)
        {
            if (SpinningPokeball != null && _viewModel.IsLoading)
            {
                await SpinningPokeball.RelRotateTo(360, 1000, Easing.Linear);
            }
            else
            {
                await Task.Delay(100);
            }
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.SearchPokemonCommand.Execute(e.NewTextValue);
    }

    private async void OnGenerationChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int generation = picker.SelectedIndex + 1;

        if (generation > 0)
        {
            await _viewModel.LoadPokemonsAsync(generation);
        }
    }

    private async void OnPokemonSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Pokemon selectedPokemon)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "PokemonData", selectedPokemon }
            };
            await Shell.Current.GoToAsync(nameof(PokemonDetailPage), navigationParameter);
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
