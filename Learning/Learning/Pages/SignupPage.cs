using System;
using Microsoft.Maui.Controls;

using Learning;
using Firebase.Database;

namespace Learning.Pages;

public class SignupPage : ContentPage
{
    Entry usernameEntry;
    Entry passwordEntry;
    
    private readonly AuthService _authService;
    private FirebaseClient _dbClient = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await DisplayAlert("Welcome", "Create a new account to get started!", "OK");
    }

    public SignupPage()
    {
        
     


        _authService = new AuthService();

        usernameEntry = new Entry { Placeholder = "Email" }; 
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