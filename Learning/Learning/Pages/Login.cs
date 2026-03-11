using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Firebase.Auth;

namespace Learning;

public class Login : ContentPage
{
    private Entry _usernameEntry; 
    private Entry _passwordEntry;
    private Label _errorLabel;
    private readonly AuthService _authService;

    public Login()
    {
        _authService = new AuthService(); 

        this.BackgroundColor = Color.FromUint(0xFF512BDF);

        var layout = new VerticalStackLayout
        {
            Margin = new Thickness(15),
            Padding = new Thickness(30, 60, 30, 30),
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

        var loginButton = new Button { Text = "Login", BackgroundColor = Color.FromRgb(0, 148, 255) };
        layout.Children.Add(loginButton);

        var signupButton = new Button { Text = "Create Account" };
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

           
            var user = await _authService.SignInAsync(email, password);

            if (user != null)
            {
             
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
               
                _errorLabel.Text = "Invalid email or password";
                _errorLabel.IsVisible = true;
            }
        };

        signupButton.Clicked += async (s, e) =>
        {
            await Navigation.PushAsync(new Pages.SignupPage());
        };
    }
}