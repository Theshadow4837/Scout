using Firebase.Database;
using Firebase.Database.Query;

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
        _listContainer.Children.Clear();
        string teamCode = Preferences.Get("MyTeamCode", "");

        
        var rawSubmissions = await _dbClient
            .Child("teams")
            .Child(teamCode)
            .Child("forms")
            .OnceAsync<TeamForm>();

        
        var grouped = rawSubmissions
            .GroupBy(s => (string)s.Object.FormTitle)
            .ToList();

        foreach (var group in grouped)
        {
            var btn = new Button
            {
                BackgroundColor = Color.FromRgb(188, 16, 16),
                Text = $"{group.Key} ({group.Count()} Submissions)",
                Margin = 10
            };

            btn.Clicked += async (s, e) => {

                var listToSend = group.Select(x => (object)x).ToList();

                if (listToSend.Count == 0) { await DisplayAlert("Error", "List is empty", "OK"); return; }

                await Navigation.PushAsync(new Pages.SubmissionViewerPage(listToSend));
            };

            _listContainer.Children.Add(btn);
        }
    }







}