using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Linq.Expressions;
using static Learning.Pages.CreateFormPage;


namespace Learning.Pages;

public class TeamForm
{

	

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("formtitle")] 
    public string FormTitle { get; set; }

    [JsonProperty("questions")] 
    public List<string> Questions { get; set; } = new List<string>();

    [JsonProperty("createdat")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public object Answers { get; internal set; }

    public TeamForm() 
	{ 
	
	}
}
public class Forms : ContentPage
{
	public readonly FirebaseClient _dbClient = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");
	private VerticalStackLayout _listContainer;


	public Forms()
	{

        this.BackgroundColor = Color.FromRgb(12, 12, 12);

        Title = "Forms";
		 
		_listContainer = new VerticalStackLayout { Padding = 20, Spacing = 10 };

		Content = new ScrollView { Content = _listContainer };
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		
		if (_listContainer != null)
		{
			await LoadForms();
        }
	}

	private async Task LoadForms()
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
				_listContainer.Children.Add(new Label { Text = "No forms found.", FontSize = 16, TextColor = Colors.Gray });
				return;
			}

			foreach (var item in firebaseResult)
			{
				var form = item.Object;

				var btn = new Button
				{
					Text = form.FormTitle ?? "Untitled Form",
                    BackgroundColor = Color.FromRgb(188, 16, 16),
					TextColor = Colors.Black,
					Margin = 5,
				};

				btn.Clicked += async (sender, e) => await Navigation.PushAsync(new FillFormPage(form));

				_listContainer.Children.Add(btn);

			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Failed to load forms: {ex.Message}", "OK");
		}
	}	
}