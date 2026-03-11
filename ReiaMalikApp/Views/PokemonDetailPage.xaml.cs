using ReiaMalikApp.ViewModels;

namespace ReiaMalikApp.Views;

public partial class PokemonDetailPage : ContentPage
{
    public PokemonDetailPage(PokemonDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}