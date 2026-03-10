using ReiaMalikApp.ViewModels;

namespace ReiaMalikApp.Views;

public partial class PokedexPage : ContentPage
{
    // On injecte le ViewModel dans le constructeur
    public PokedexPage(PokedexViewModel viewModel)
    {
        InitializeComponent();

        // On définit le ViewModel comme contexte de données de la page
        BindingContext = viewModel;
    }
}