using Firebase.Database;
using Learning;
using Learning.Pages;

public class FillFormPage : ContentPage
{
	private List<Entry> _answerEntries = new List<Entry>();
	private Learning.TeamForm _selectedForm;
    private Learning.Pages.TeamForm form;

    public FillFormPage(Learning.TeamForm form)
	{

		_selectedForm = form;
		Title = form.FormTitle;

var layout = new VerticalStackLayout { Padding = 20, Spacing = 15 };

		foreach (var question in form.Questions)
		{
			layout.Children.Add(new Label{ Text = question, FontSize = 18 });

			var answerEntry = new Entry { Placeholder = "Your answer here" };
			_answerEntries.Add(answerEntry);
			layout.Children.Add(answerEntry);
        }

		var submitBtn = new Button { Text = "Submit", BackgroundColor = Colors.Green, Margin = new Thickness(0, 20, 0, 0) };
		submitBtn.Clicked += async (s, e) => await SubmitAnswers();

		layout.Children.Add(submitBtn);
		Content = new ScrollView { Content = layout };

	
	}

    public FillFormPage(Learning.Pages.TeamForm form)
    {
        this.form = form;
    }

    private async Task SubmitAnswers()
	{
		var answers = _answerEntries.Select(a => a.Text).ToList();

		await DisplayAlert("Submitted", "Your answers have been submitted!", "OK");
		await Navigation.PopAsync();
    }
}