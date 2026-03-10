using ReiaMalikApp.Views;

namespace ReiaMalikApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // On enregistre la route cachée pour pouvoir naviguer vers la page de détails
        Routing.RegisterRoute(nameof(PokemonDetailPage), typeof(PokemonDetailPage));

        // (Si tu as aussi créé la page GifPage pour l'étape 2 via un routeur Shell au lieu du Navigation.PushAsync standard, tu pourrais aussi l'enregistrer ici, mais ce n'est pas obligatoire selon la méthode qu'on a utilisée)
    }
}