using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;

namespace Learning;

public class TeamPage : ContentPage
{
    private string _dbUrl = "https://test-3b247-default-rtdb.firebaseio.com/";
    private readonly FirebaseClient _dbClient;
    private readonly string _userId;

    public TeamPage(string userId)
    {
        _userId = userId;
        _dbClient = new FirebaseClient(_dbUrl);
        Title = "Teams";

        // Join Section UI
        var codeEntry = new Entry { Placeholder = "Enter Team # to Join", Keyboard = Keyboard.Numeric };
        var joinBtn = new Button { Text = "Join Existing Team", BackgroundColor = Colors.Blue };

        // Create Section UI
        var goToCreateBtn = new Button { Text = "Create a New Team", BackgroundColor = Colors.Green, Margin = new Thickness(0, 20, 0, 0) };

        // Navigate to the creation page
        goToCreateBtn.Clicked += async (s, e) =>
        {
            await Navigation.PushAsync(new Pages.CreateTeamPage(_userId));
        };

        // Join Logic
        joinBtn.Clicked += async (s, e) =>
        {
            var code = codeEntry.Text?.Trim();
            if (string.IsNullOrEmpty(code)) return;

            var team = await _dbClient.Child("teams").Child(code).OnceSingleAsync<Pages.Team>();
            if (team != null)
            {
                if (!team.Members.Contains(_userId))
                {
                    team.Members.Add(_userId);
                    await _dbClient.Child("teams").Child(code).PutAsync(team);
                    await DisplayAlert("Success", $"Joined {team.TeamName}!", "OK");

                    Preferences.Set("MyTeamCode", code);

                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}"); 
                }
                else { await DisplayAlert("Info", "Already a member.", "OK"); }
            }
            else { await DisplayAlert("Error", "Team not found.", "OK"); }
        };

        Content = new VerticalStackLayout
        {
            Padding = 30,
            Spacing = 15,
            Children = {
                new Label { Text = "Join a Team", FontSize = 20, FontAttributes = FontAttributes.Bold },
                codeEntry, joinBtn,
                new BoxView { HeightRequest = 1, Color = Colors.Gray, Margin = 10 },
                new Label { Text = "Want to start your own?", FontSize = 20, FontAttributes = FontAttributes.Bold },
                goToCreateBtn
            }
        };
    }
}