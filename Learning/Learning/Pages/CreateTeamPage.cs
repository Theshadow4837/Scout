using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;


namespace Learning.Pages;


public class Team
{
	public string TeamCode { get; set; }
	public string TeamName { get; set; }
	public string CreatorId { get; set; }
	public List<string> Members { get; set; } = new List<string>();
}



    public class CreateTeamPage : ContentPage
{
	private string _dbUrl = "https://test-3b247-default-rtdb.firebaseio.com/";
	private FirebaseClient _dbClient;
	private readonly string _userId;


    public CreateTeamPage(string userId)
	{
		_userId = userId;
		_dbClient = new FirebaseClient(_dbUrl);
		Title = "Create a Team";

		var nameEntry = new Entry { Placeholder = "Team Name (e.g. Hephaustus)" };
		var numberEntry = new Entry { Placeholder = "Team # (e.g. 6390)", Keyboard = Keyboard.Numeric };
		var saveBtn = new Button { Text = "Create Team", BackgroundColor = Colors.Green };

		
		saveBtn.Clicked += async (s, e) =>
		{
			string name = nameEntry.Text?.Trim();
			string num = numberEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(num))
            {
                
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            var existingTeam = await _dbClient.Child("teams").Child(num).OnceSingleAsync<Team>();

			if (existingTeam != null) {

				await DisplayAlert("Error", "Team # already exists. Please choose a different number.", "OK");
			}
			else
			{
				var newTeam = new Team
				{
					TeamCode = num,
					TeamName = name,
					CreatorId = _userId,
					Members = new List<string> { _userId }
				};

				await _dbClient.Child("teams").Child(num).PutAsync(newTeam);
				await DisplayAlert("Success", $"Team {name} created!", "OK");


				Application.Current.MainPage = new NavigationPage(new MainPage());

            }
		};




		Content = new VerticalStackLayout
		{
			Padding = 30,
			Spacing = 20,
			Children = {
				new Label { Text = "Team Name", FontSize = 24, FontAttributes = FontAttributes.Bold },
				nameEntry,

                new Label { Text= "Team #", FontSize = 24, FontAttributes = FontAttributes.Bold },
				numberEntry,

				saveBtn
		}
		};
		}
	}