using Microsoft.Extensions.Logging;
using ReiaMalikApp.Services;
using ReiaMalikApp.ViewModels;
using ReiaMalikApp.Views;

namespace ReiaMalikApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // --- 1. ENREGISTREMENT DES SERVICES (LOGIQUE) ---
        // On utilise Singleton car on a besoin d'une seule instance de l'API pour toute l'app
        builder.Services.AddSingleton<PokeApiService>();

        // --- 2. ENREGISTREMENT DU POKEDEX (LISTE) ---
        builder.Services.AddTransient<PokedexViewModel>();
        builder.Services.AddTransient<PokedexPage>();

        // --- 3. ENREGISTREMENT DES DETAILS (CLIC SUR ITEM) ---
        // Ces lignes corrigent ton erreur "Unable to resolve service"
        builder.Services.AddTransient<PokemonDetailViewModel>();
        builder.Services.AddTransient<PokemonDetailPage>();

        // --- 4. ENREGISTREMENT DU FORMULAIRE (AJOUT) ---
        // On anticipe l'Étape 5 en enregistrant déjà la page d'ajout
        builder.Services.AddTransient<AddPokemonPage>();

        return builder.Build();
    }
}