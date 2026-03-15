using Firebase.Database;
using Firebase.Database.Query;
using Learning.Pages;

namespace Learning.Pages;

public class FormDataPage : ContentPage
{
	private readonly FirebaseClient _dbClient = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");
	private VerticalStackLayout _listContainer;
    public FormDataPage()
	{

        this.BackgroundColor = Color.FromRgb(12, 12, 12);

        Title = "Form Data";

		_listContainer = new VerticalStackLayout();

		Content = new ScrollView
		{
			Content = new VerticalStackLayout
			{
				Children =
			{
				new Label { Text = "Form Data", FontSize = 40, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 20) },
				_listContainer
			}
			}
		};    
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadFormData();
    }

	private async Task LoadFormData()
	{
		try
		{
			_listContainer.Children.Clear();
			string teamCode = Preferences.Get("MyTeamCode", Preferences.Get("teamCode", null));
			var firebaseResult = await _dbClient
				.Child("teams")
				.Child(teamCode)
				.Child("forms")
				.OnceAsync<TeamForm>();
			if (firebaseResult == null || !firebaseResult.Any())
			{
				_listContainer.Children.Add(new Label { Text = "No form data found.", FontSize = 16, TextColor = Colors.Gray });
				return;
			}
            foreach (var item in firebaseResult)
            {
                var data = item.Object;

                // Create a clickable card for each submission
                var frame = new Frame
                {
                    HasShadow = true,
                    BorderColor = Colors.LightGray,
                    CornerRadius = 8,
                    Padding = 15,
                    Margin = new Thickness(0, 5),
                    BackgroundColor = Color.FromArgb("#212121"), // Dark theme friendly
                    Content = new VerticalStackLayout
                    {
                        Children = {
                            new Label { Text = (string)data.FormTitle, FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Colors.White },
                            new Label { Text = $"By: {data.Id}", FontSize = 14, TextColor = Colors.LightGray },
                            new Label { Text = $"Date: {data.CreatedAt}", FontSize = 12, TextColor = Colors.Gray }
                        }
                    }
                };
                _listContainer.Children.Add(frame);
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Failed to load form data: {ex.Message}", "OK");
		}
	}







}