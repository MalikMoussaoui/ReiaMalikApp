using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using ReiaMalikApp.Models;

namespace ReiaMalikApp.Views;

public partial class BonusPage : ContentPage
{
    private readonly string[] _wildNames = { "Roucool", "Rattata", "Piafabec", "Abo", "Nosferapti", "Mystherbe", "Pikachu", "Evoli" };

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
                return;
            }

            if (userLocation != null)
            {
                StatusLabel.Text = "Pokémon sauvages détectés !";

                var mapSpan = new MapSpan(userLocation, 0.01, 0.01);
                PokemonMap.MoveToRegion(mapSpan);

                SpawnPokemons(userLocation);
            }
            else
            {
                StatusLabel.Text = "Impossible de te localiser. Réessaie.";
            }
        }
        else
        {
            StatusLabel.Text = "Permission GPS refusée. Impossible de jouer.";
        }
    }

    private void SpawnPokemons(Location center)
    {
        PokemonMap.Pins.Clear();
        var random = new Random();

        for (int i = 0; i < 5; i++)
        {
            double latOffset = (random.NextDouble() * 0.009) - 0.0045;
            double lonOffset = (random.NextDouble() * 0.009) - 0.0045;

            var pinLocation = new Location(center.Latitude + latOffset, center.Longitude + lonOffset);
            var pkmName = _wildNames[random.Next(_wildNames.Length)];

            var pin = new Pin
            {
                Label = pkmName,
                Address = "Appuie pour capturer !",
                Type = PinType.Place,
                Location = pinLocation
            };

            pin.MarkerClicked += async (s, args) =>
            {
                args.HideInfoWindow = true;
                await CatchPokemon(pin, pkmName);
            };

            PokemonMap.Pins.Add(pin);
        }
    }

    private async Task CatchPokemon(Pin pin, string name)
    {
        bool throwPokeball = await DisplayAlert("Rencontre Sauvage !", $"Un {name} sauvage apparaît. Veux-tu lancer une Pokéball ?", "Lancer !", "Fuir");

        if (throwPokeball)
        {
            int catchRate = new Random().Next(100);

            if (catchRate > 30) // 70% de chance d'attraper le Pokémon
            {
                await DisplayAlert("Super !", $"Tu as attrapé {name} ! Il a été transféré au Pokédex.", "Génial");
                PokemonMap.Pins.Remove(pin);

                Pokemon.Captured.Add(new Pokemon
                {
                    Name = name,
                    Type1 = "NORMAL",
                    Generation = 6,
                    ImageUrl = "pokeball_logo.png",
                    Description = $"Attrapé dans la nature. Ses statistiques sont mystérieuses...",
                    Category = "Sauvage"
                });
            }
            else
            {
                await DisplayAlert("Zut !", $"Le {name} s'est échappé... Il a fui très loin.", "OK");
                PokemonMap.Pins.Remove(pin);
            }
        }
    }
}