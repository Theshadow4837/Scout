using System;
using Microsoft.Maui.Controls;
using Learning;

namespace Learning.Pages;

public class SignupPage : ContentPage
{
    Entry usernameEntry;
    Entry passwordEntry;
    private readonly AuthService _authService;

    public SignupPage()
    {
        _authService = new AuthService();

        usernameEntry = new Entry { Placeholder = "Email" }; // Changed placeholder to 'Email'
        passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };

        Button createButton = new Button { Text = "Create Account" };

        createButton.Clicked += async (s, e) =>
        {
            string email = usernameEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }
            var result = await _authService.SignUpAsync(email, password);

            if (result != null)
            {
                await DisplayAlert("Success", "Account created in Firebase!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Registration failed. Check your connection or email format.", "OK");
            }
        };

        Content = new VerticalStackLayout
        {
            Padding = 30,
            Children =
            {
                usernameEntry,
                passwordEntry,
                createButton
            }
        };
    }
}