using Microsoft.Extensions.DependencyInjection;
namespace Learning
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(FillFormPage), typeof(FillFormPage));


        }


        private async void OnFormsClicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new Pages.Forms());
        }


        private async void OnLogoutClicked(object sender, EventArgs e)
        {

            Preferences.Clear();

            await Shell.Current.GoToAsync($"//{nameof(Login)}");
        }

        private void MenuFlyoutItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}