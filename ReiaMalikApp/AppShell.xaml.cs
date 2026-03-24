using ReiaMalikApp.Views;

namespace ReiaMalikApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Views.PokemonDetailPage), typeof(Views.PokemonDetailPage));
    }
}