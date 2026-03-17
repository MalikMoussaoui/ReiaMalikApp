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
            if (photo != null)
            {
                // On met le chemin du fichier local dans l'Entry, l'application saura le lire !
                ImageUrlEntry.Text = photo.FullPath;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", "Impossible d'ouvrir la galerie photo.", "OK");
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) || TypePicker.SelectedIndex == -1)
        {
            await DisplayAlert("Erreur", "Veuillez entrer un nom et choisir un type.", "OK");
            return;
        }

        var newPokemon = new Pokemon
        {
            Name = NameEntry.Text,
            Type = TypePicker.SelectedItem.ToString(),
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

        // On sauvegarde dans la "mémoire" de la Génération S
        Pokemon.GenerationS.Add(newPokemon);

        await DisplayAlert("Succès", $"{newPokemon.Name} a rejoint la Génération S !", "Génial");

        await Shell.Current.GoToAsync("..");
    }
}
