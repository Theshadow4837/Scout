using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Firebase.Auth;
using Learning.Pages;
using Firebase.Database;

namespace Learning;


public class TeamForm
{
    public string FormTitle { get; set; }
    public List<string> Questions { get; set; } = new List<string>();
    public string Id { get; internal set; }
}


public class Login : ContentPage
{
    private Entry _usernameEntry;
    private Entry _passwordEntry;
    private Label _errorLabel;
    private readonly AuthService _authService;
    string savedTeamCode = Preferences.Get("teamCode", null);

    private FirebaseClient _dbClient = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");
    public Login()
    {
        _authService = new AuthService();

        this.BackgroundColor = Color.FromRgb(12,12,12);
        

        var layout = new VerticalStackLayout
        {
            Margin = new Thickness(15),
            Padding = new Thickness(30, 60, 30, 30),
            Spacing = 2,
            Children =
            {
                new Label { Text = "Please log in", FontSize = 30, TextColor = Colors.White },
                new Label { Text = "Email", TextColor = Colors.White }
            }
        };

        _usernameEntry = new Entry { Placeholder = "Enter email" };
        layout.Children.Add(_usernameEntry);

        layout.Children.Add(new Label { Text = "Password", TextColor = Colors.White });
        _passwordEntry = new Entry { IsPassword = true, Placeholder = "Enter password" };
        layout.Children.Add(_passwordEntry);


        _errorLabel = new Label { TextColor = Colors.Red, IsVisible = false };
        layout.Children.Add(_errorLabel);

        var loginButton = new Button { Text = "Login", BackgroundColor = Color.FromRgb(188, 16, 16) };
        layout.Children.Add(loginButton);

        var signupButton = new Button { Text = "Create Account", BackgroundColor = Color.FromRgb(188, 16, 16) };
        layout.Children.Add(signupButton);

        Content = layout;


        loginButton.Clicked += async (sender, e) =>
        {
            string email = _usernameEntry.Text?.Trim();
            string password = _passwordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _errorLabel.Text = "Enter email and password";
                _errorLabel.IsVisible = true;
                return;
            }

            signupButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new SignupPage());
            };

            var user = await _authService.SignInAsync(email, password);

            if (user != null)
            {
                string uid = user.User.Uid;
                Preferences.Set("userId", uid);

                
                string localCode = Preferences.Get("MyTeamCode", "");

                if (!string.IsNullOrWhiteSpace(localCode))
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                   
                    var allTeams = await _dbClient.Child("teams").OnceAsync<Team>();

                    var myTeam = allTeams.FirstOrDefault(t =>
                        t.Object.Members != null && t.Object.Members.Contains(uid));

                    if (myTeam != null)
                    {
                        
                        Preferences.Set("MyTeamCode", myTeam.Object.TeamCode);
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    }
                    else
                    {
                        
                        await Navigation.PushAsync(new TeamPage(uid));
                    }
                }
            }
        };
        }
}