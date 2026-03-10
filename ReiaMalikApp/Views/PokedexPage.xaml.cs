using ReiaMalikApp.ViewModels;

namespace ReiaMalikApp.Views;

public partial class PokedexPage : ContentPage
{
    public PokedexPage(PokedexViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}