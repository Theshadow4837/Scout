namespace Learning;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		this.BackgroundColor = Color.FromRgb(12, 12, 12);

        var titleLabel = new Label
		{
			Text = "Team Dashboard",
			FontSize = 40,
			HorizontalOptions = LayoutOptions.Center,
			Margin = new Thickness(0, 20, 0, 10)
        };

		var createFormBtn = new Button
		{
			Text = "Create Form",
            BackgroundColor = Color.FromRgb(188, 16, 16),
			TextColor = Colors.White,
			Margin = new Thickness(0, 10, 0, 0)
		};

		var viewFormsBtn = new Button
		{
			Text = "View Forms",
            BackgroundColor = Color.FromRgb(188, 16, 16),
			TextColor = Colors.White,
			Margin = new Thickness(0, 10, 0, 0)
		};

        var viewDataBtn = new Button
		{
			Text = "View Data",
            BackgroundColor = Color.FromRgb(188, 16, 16),
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