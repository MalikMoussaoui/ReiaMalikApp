using Microsoft.Extensions.Logging;

namespace ReiaMalikApp
{
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
            builder.Services.AddSingleton<ReiaMalikApp.Services.PokeApiService>();
            builder.Services.AddTransient<ReiaMalikApp.ViewModels.PokedexViewModel>();
            builder.Services.AddTransient<ReiaMalikApp.Views.PokedexPage>();

            return builder.Build();
        }
    }
}
