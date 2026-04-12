using Mapsui.UI.Maui;
using Mapsui.Projections;
using ReiaMalikApp.Models;
using Mapsui;
using Mapsui.Tiling;
using System.Net.Http.Json;

namespace ReiaMalikApp.Views;

public partial class BonusPage : ContentPage
{
    private class WildPokemonInfo
    {
        public string Name { get; set; }
        public string SpriteUrl { get; set; }
    }

    private List<WildPokemonInfo> _allPokemons = new();
    private Location _lastLocation;
    private readonly HttpClient _httpClient = new HttpClient();

    public BonusPage()
    {
        InitializeComponent();

        PokemonMap.Map?.Layers.Add(OpenStreetMap.CreateTileLayer());
        PokemonMap.Map?.Widgets.Clear();

        PokemonMap.PinClicked += OnPokemonClicked;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPokemonRoster();
        await StartSafariMode();
    }

    private async Task LoadPokemonRoster()
    {
        if (_allPokemons.Any()) return;

        StatusLabel.Text = "Connexion au Pokédex Mondial...";
        try
        {
            var url = "https://pokeapi.co/api/v2/pokemon?limit=1000";
            var response = await _httpClient.GetFromJsonAsync<PokeApiResponse>(url);

            if (response?.Results != null)
            {
                foreach (var p in response.Results)
                {
                    var id = p.Url.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
                    _allPokemons.Add(new WildPokemonInfo
                    {
                        Name = char.ToUpper(p.Name[0]) + p.Name.Substring(1),
                        SpriteUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png"
                    });
                }
            }
        }
        catch
        {
            _allPokemons = new List<WildPokemonInfo>
            {
                new WildPokemonInfo { Name = "Pikachu", SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png" }
            };
        }
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
                StatusLabel.Text = "Mode Simulation GPS (Paris)";
                userLocation = new Location(48.8566, 2.3522);
            }

            if (userLocation != null)
            {
                StatusLabel.Text = "Pokémon sauvages détectés !";
                _lastLocation = userLocation;

                PokemonMap.MyLocationEnabled = true;
                PokemonMap.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Maui.Position(userLocation.Latitude, userLocation.Longitude));

                var coords = SphericalMercator.FromLonLat(userLocation.Longitude, userLocation.Latitude);
                var center = new MPoint(coords.x, coords.y);

                PokemonMap.Map.Navigator.CenterOn(center);
                PokemonMap.Map.Navigator.ZoomTo(2);

                SpawnPokemons(_lastLocation);
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
            double latOffset = (random.NextDouble() * 0.009) - 0.0045;
            double lonOffset = (random.NextDouble() * 0.009) - 0.0045;

            var pkm = _allPokemons[random.Next(_allPokemons.Count)];

            var pin = new Mapsui.UI.Maui.Pin(PokemonMap)
            {
                Position = new Mapsui.UI.Maui.Position(center.Latitude + latOffset, center.Longitude + lonOffset),
                Label = pkm.Name,
                Type = Mapsui.UI.Maui.PinType.Pin,
                Color = Colors.DarkGreen,
                Scale = 0.7f,
                Tag = pkm.SpriteUrl
            };

            PokemonMap.Pins.Add(pin);
        }
    }

    private async void OnPokemonClicked(object sender, PinClickedEventArgs args)
    {
        if (args.Pin != null)
        {
            args.Handled = true;
            await CatchPokemon((Mapsui.UI.Maui.Pin)args.Pin, args.Pin.Label);
        }
    }

    private async Task CatchPokemon(Mapsui.UI.Maui.Pin pin, string name)
    {
        bool throwPokeball = await DisplayAlert("Rencontre Sauvage !", $"Un {name} sauvage apparaît ! Lancer une Pokéball ?", "Lancer !", "Fuir");

        if (throwPokeball)
        {
            int catchRate = new Random().Next(100);
            if (catchRate > 30)
            {
                await DisplayAlert("Félicitations !!!", $"Tu as attrapé {name} !", "Génial");
                PokemonMap.Pins.Remove(pin);

                string imageUrl = pin.Tag as string ?? "pokeball_logo.png";

                Pokemon.Captured.Add(new Pokemon
                {
                    Name = name,
                    Type1 = "NORMAL",
                    Generation = 6,
                    ImageUrl = imageUrl,
                    Description = "Attrapé dans la nature avec le GPS.",
                    Category = "Sauvage"
                });
            }
            else
            {
                await DisplayAlert("Mince !", $"{name} s'est échappé... Peut-être une prochaine fois !", "OK");
                PokemonMap.Pins.Remove(pin);
            }
        }

        if (PokemonMap.Pins.Count == 0 && _lastLocation != null)
        {
            await DisplayAlert("Safari Zone", "Les herbes bougent... de nouveaux Pokémon apparaissent !", "A la chasse !");
            SpawnPokemons(_lastLocation);
        }
    }
}