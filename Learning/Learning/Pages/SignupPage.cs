using System;
using Microsoft.Maui.Controls;
// Make sure to include the namespace where your AuthService is located
using Learning;
using Firebase.Database;

namespace Learning.Pages;

public class SignupPage : ContentPage
{
    Entry usernameEntry;
    Entry passwordEntry;
    // 1. Create a field for your Auth Service
    private readonly AuthService _authService;
    private FirebaseClient _dbClient = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");

    public SignupPage()
    {
        // 2. Initialize the service
        _authService = new AuthService();

        usernameEntry = new Entry { Placeholder = "Email" }; // Changed placeholder to 'Email'
        passwordEntry = new Entry { Placeholder = "Password", IsPassword = true };

        Button createButton = new Button { Text = "Create Account" };

        // 3. Make the event handler 'async'
        createButton.Clicked += async (s, e) =>
        {
            string email = usernameEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            // 4. Call the Firebase SignUp method
            var result = await _authService.SignUpAsync(email, password);

            if (result != null)
            {
                // Success!
                await DisplayAlert("Success", "Account created in Firebase!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                // Failure
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