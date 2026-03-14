namespace Learning;

public partial class MainPage : ContentPage
{
	public MainPage()
	{


		var titleLabel = new Label
		{
			Text = "Team Dashboard",
			FontSize = 28,
			HorizontalOptions = LayoutOptions.Center,
			Margin = new Thickness(0, 20, 0, 10)
        };

		var createFormBtn = new Button
		{
			Text = "Create Form",
			BackgroundColor = Colors.Green,
			TextColor = Colors.White,
			Margin = new Thickness(0, 10, 0, 0)
		};

		var viewFormsBtn = new Button
		{
			Text = "View Forms",
			BackgroundColor = Colors.Blue,
			TextColor = Colors.White,
			Margin = new Thickness(0, 10, 0, 0)
		};

        var viewDataBtn = new Button
		{
			Text = "View Data",
			BackgroundColor = Colors.Green,
			TextColor = Colors.White,
			Margin = new Thickness(0, 10, 0, 0)
		};

		createFormBtn.Clicked += async (s, e) =>
		{
			await Navigation.PushAsync(new Pages.CreateFormPage());
		};
		
		viewFormsBtn.Clicked += async (s, e) =>
		{
			await Shell.Current.Navigation.PushAsync(new Pages.Forms());
		};


        viewDataBtn.Clicked += async (s, e) =>
		{
			await Navigation.PushAsync(new Pages.FormDataPage());
		};









        Content = new VerticalStackLayout
		{
			Padding = new Thickness(30),
			Spacing = 20,
            Children = {
				titleLabel,
				new BoxView { HeightRequest = 1, Color = Colors.Gray },
				createFormBtn,
				viewFormsBtn,
				viewDataBtn
            }
		};
	}
}