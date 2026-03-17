using Mapsui.UI.Maui;
using Mapsui.Projections;
using ReiaMalikApp.Models;
using Mapsui;

namespace ReiaMalikApp.Views;

public partial class BonusPage : ContentPage
{
    private readonly string[] _wildNames = { "Roucool", "Rattata", "Piafabec", "Abo", "Nosferapti", "Pikachu", "Evoli" };

    public BonusPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await StartSafariMode();
    }

    private async Task StartSafariMode()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        if (status == PermissionStatus.Granted)
        {
            StatusLabel.Text = "Recherche de ta vraie position...";
            Location userLocation = null;

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                userLocation = await Geolocation.Default.GetLocationAsync(request);
            }
            catch
            {
                StatusLabel.Text = "Erreur : Active le GPS de ton appareil !";
                // Mode simulation pour l'émulateur Android si le GPS n'est pas configuré
                userLocation = new Location(48.8566, 2.3522); // Paris
            }

            if (userLocation != null)
            {
                StatusLabel.Text = "Pokémon sauvages détectés !";

                // Centrer la carte OpenStreetMap sur ta position
                var center = SphericalMercator.FromLonLat(userLocation.Longitude, userLocation.Latitude);
                PokemonMap.Map.Navigator.NavigateTo(center, PokemonMap.Map.Resolutions[15]);

                SpawnPokemons(userLocation);
            }
        }
        else
        {
            StatusLabel.Text = "Permission GPS refusée.";
        }
    }

    private void SpawnPokemons(Location center)
    {
        PokemonMap.Pins.Clear();
        var random = new Random();

        for (int i = 0; i < 5; i++)
        {
            // Rayon autour de ta position
            double latOffset = (random.NextDouble() * 0.009) - 0.0045;
            double lonOffset = (random.NextDouble() * 0.009) - 0.0045;

            var pkmName = _wildNames[random.Next(_wildNames.Length)];

            // Création du Pin façon Mapsui
            var pin = new Pin(PokemonMap)
            {
                Position = new Position(center.Latitude + latOffset, center.Longitude + lonOffset),
                Label = pkmName,
                Type = PinType.Pin,
                Color = Colors.Red
            };

            PokemonMap.Pins.Add(pin);
        }

        // Quand tu cliques sur un Pokémon
        PokemonMap.PinClicked += async (s, args) =>
        {
            if (args.Pin != null)
            {
                args.Handled = true; // Bloque les infos bulles par défaut
                await CatchPokemon(args.Pin, args.Pin.Label);
            }
        };
    }

    private async Task CatchPokemon(Pin pin, string name)
    {
        bool throwPokeball = await DisplayAlert("Rencontre Sauvage !", $"Un {name} sauvage apparaît. Lancer une Pokéball ?", "Lancer !", "Fuir");

        if (throwPokeball)
        {
            int catchRate = new Random().Next(100);
            if (catchRate > 30) // 70% de chance d'attraper
            {
                await DisplayAlert("Super !", $"Tu as attrapé {name} !", "Génial");
                PokemonMap.Pins.Remove(pin);

                Pokemon.Captured.Add(new Pokemon
                {
                    Name = name,
                    Type1 = "NORMAL",
                    Generation = 6,
                    ImageUrl = "pokeball_logo.png",
                    Description = "Attrapé dans la nature avec le GPS.",
                    Category = "Sauvage"
                });
            }
            else
            {
                await DisplayAlert("Zut !", $"Le {name} s'est échappé...", "OK");
                PokemonMap.Pins.Remove(pin);
            }
        }
    }
}