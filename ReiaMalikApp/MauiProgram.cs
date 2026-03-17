using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ReiaMalikApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            // On active SkiaSharp ici pour que la carte ne fasse plus crash l'appli :
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<Services.PokeApiService>();
        builder.Services.AddTransient<ViewModels.PokedexViewModel>();
        builder.Services.AddTransient<Views.PokedexPage>();
        builder.Services.AddTransient<ViewModels.PokemonDetailViewModel>();
        builder.Services.AddTransient<Views.PokemonDetailPage>();

        return builder.Build();
    }
}