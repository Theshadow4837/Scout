using Firebase.Database;
using Firebase.Database.Query;
using Learning;
using Learning.Pages;


public class FillFormPage : ContentPage
{

	private readonly Learning.Pages.TeamForm _selectedForm;
	private readonly List<Entry> _answerEntries = new List<Entry>();
	private readonly FirebaseClient _dbClient = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");

    public FillFormPage(Learning.Pages.TeamForm form)
	{

        this.BackgroundColor = Color.FromRgb(12, 12, 12);

        if (form == null)
		{
			Content = new Label { Text = "Form data is missing.", TextColor = Colors.Red, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
			return;
        }

		_selectedForm = form;
		Title = form.FormTitle ?? "Fill Form";

		var mainstack = new VerticalStackLayout { Padding = 20, Spacing = 15 };

		if (form.Questions != null)
		{
			foreach (var question in form.Questions)
			{
				var qLabel = new Label { Text = question, FontSize = 16, FontAttributes = FontAttributes.Bold };

				var answerEntry = new Entry { Placeholder = "Your answer here" };

				_answerEntries.Add(answerEntry);
				mainstack.Children.Add(qLabel);
				mainstack.Children.Add(answerEntry);
            }
		}

		var submitBtn = new Button { Text = "Submit Answers", BackgroundColor = Color.FromRgb(188, 16, 16), TextColor = Colors.White, Margin = new Thickness(0, 20, 0, 0) };

		submitBtn.Clicked += OnSubmitClicked;
		mainstack.Children.Add(submitBtn);

		Content = new ScrollView { Content = mainstack };

    }

	private async void OnSubmitClicked(object sender, EventArgs e)
	{
		if (_answerEntries.Any(x => string.IsNullOrEmpty(x.Text)))
		{
			await DisplayAlert("Validation Error", "Please answer all questions before submitting.", "OK");
			return;
        }


		try
		{
			var teamCode = Preferences.Get("MyTeamCode", null);
			var userId = Preferences.Get("userId", "Anonymous");

			var userAnswers = _answerEntries.Select(entry => entry.Text ?? "").ToList();

			var submission = new
			{
				FormId = _selectedForm.Id,
				FormTitle = _selectedForm.FormTitle,
				User = userId,
				Timstamp = DateTime.UtcNow,
				Answers = userAnswers,
			};

			await _dbClient
				.Child("teams")
				.Child(teamCode)
				.Child("formSubmissions")
				.PostAsync(submission);

			await DisplayAlert("Success", "Your answers have been submitted!", "OK");
			await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Failed to submit answers: {ex.Message}", "OK");
        }
    }

}