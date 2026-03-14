using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls;
using System;

namespace Learning.Pages;

public class CreateFormPage : ContentPage
{

	private VerticalStackLayout _questionsContainer;
	private List<Entry> _questionEntries = new List<Entry>();
	private Entry _formTitleEntry;
	private FirebaseClient _dbClient = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");

    public CreateFormPage()
	{
		Title = "Create Form";


        _formTitleEntry = new Entry
        {
            Placeholder = "Form Title (e.g., Weekly Check-in)",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 0, 0, 20)
        };


        var header = new Label
		{
			Text = "Form Builder",
			FontSize = 24,
			HorizontalOptions = LayoutOptions.Center,
		};

		_questionsContainer = new VerticalStackLayout { Spacing = 10 };

		var addQuestionBtn = new Button
		{
			Text = "Add Question",
			BackgroundColor = Colors.Green,
			TextColor = Colors.White,
		};

		var saveFormBtn = new Button
		{
			Text = "Publish Form",
			Margin = new Thickness(0, 20, 0, 0),
		};


		addQuestionBtn.Clicked += (s, e) =>
		{
			var newQuestion = new Entry { Placeholder = $"Question {_questionEntries.Count + 1}" };
			_questionEntries.Add(newQuestion);
			_questionsContainer.Children.Add(newQuestion);
		};

		saveFormBtn.Clicked += async (s, e) => await SaveFormToFirebase();




		Content = new ScrollView
		{

			Content = new VerticalStackLayout
			{
				Padding = 30,
				Spacing = 15,
				Children = {
					header,
					new Label { Text = "Form Title", FontSize = 18, FontAttributes = FontAttributes.Bold },
					_formTitleEntry,
					new BoxView { HeightRequest = 1, Color = Colors.Gray, Margin = new Thickness(0, 10) },
                    _questionsContainer,
					addQuestionBtn, 
					saveFormBtn }


			}

		};

	}

	private async Task SaveFormToFirebase()
	{
		string teamCode = Preferences.Get("MyTeamCode", null);
		if (string.IsNullOrEmpty(teamCode)) return;

		var newForm = new TeamForm
		{
			Id = Guid.NewGuid().ToString(),
			FormTitle = _formTitleEntry.Text,
			Questions = _questionEntries.Select(x => x.Text).ToList()
		};

		

        await _dbClient
        .Child("teams")
        .Child(teamCode)
        .Child("forms")
        .Child(newForm.Id)
        .PutAsync(newForm);

        await DisplayAlert("Success", "Form published successfully!", "OK");
		await Navigation.PopAsync();
    }

   
};
	
