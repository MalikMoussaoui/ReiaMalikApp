using ReiaMalikApp.Models;

namespace ReiaMalikApp.Views;

public partial class AddPokemonPage : ContentPage
{
    public AddPokemonPage()
    {
        InitializeComponent();
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null) ImageUrlEntry.Text = photo.FullPath;
        }
        catch { await DisplayAlert("Erreur", "Impossible d'ouvrir la galerie.", "OK"); }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var pokemonName = NameEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(pokemonName) || Type1Picker.SelectedIndex == -1)
        {
            await DisplayAlert("Erreur", "Veuillez entrer un nom et choisir un type principal.", "OK");
            return;
        }

        if (Pokemon.GenerationS.Any(p => p.Name.Equals(pokemonName, StringComparison.OrdinalIgnoreCase)))
        {
            await DisplayAlert("Erreur", $"Le Pokémon '{pokemonName}' existe déjà !", "OK");
            return;
        }

        string type2 = "";
        if (Type2Picker.SelectedIndex != -1 && Type2Picker.SelectedItem.ToString() != "AUCUN")
        {
            type2 = Type2Picker.SelectedItem.ToString();
        }

        string type1 = Type1Picker.SelectedItem.ToString();
        if (type1 == type2)
        {
            type2 = "";
        }

        var newPokemon = new Pokemon
        {
            Name = pokemonName,
            Type1 = type1,
            Type2 = type2,
            Generation = 5,
            ImageUrl = string.IsNullOrWhiteSpace(ImageUrlEntry.Text) ? "pokeball_logo.png" : ImageUrlEntry.Text,
            Description = string.IsNullOrWhiteSpace(DescriptionEditor.Text) ? "Aucune description." : DescriptionEditor.Text,
            HP = string.IsNullOrWhiteSpace(HpEntry.Text) ? "0" : HpEntry.Text,
            Attack = string.IsNullOrWhiteSpace(AtkEntry.Text) ? "0" : AtkEntry.Text,
            Defense = string.IsNullOrWhiteSpace(DefEntry.Text) ? "0" : DefEntry.Text,
            Height = string.IsNullOrWhiteSpace(HeightEntry.Text) ? "???" : HeightEntry.Text + " m",
            Weight = string.IsNullOrWhiteSpace(WeightEntry.Text) ? "???" : WeightEntry.Text + " kg",
            Category = "Génération S",
            Ability = "Talent Spécial"
        };

        Pokemon.GenerationS.Add(newPokemon);
        await DisplayAlert("Succès", $"{newPokemon.Name} a rejoint la Génération S !", "Génial");

        NameEntry.Text = string.Empty;
        Type1Picker.SelectedIndex = -1;
        Type2Picker.SelectedIndex = -1;
        ImageUrlEntry.Text = string.Empty;
        DescriptionEditor.Text = string.Empty;
        HpEntry.Text = "50";
        AtkEntry.Text = "50";
        DefEntry.Text = "50";
        HeightEntry.Text = "1.0";
        WeightEntry.Text = "10.0";

        await Shell.Current.GoToAsync("..");
    }
}
