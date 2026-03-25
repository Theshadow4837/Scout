using Microsoft.Extensions.DependencyInjection;
namespace Learning
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(Pages.FillFormPage), typeof(Pages.FillFormPage));
            Routing.RegisterRoute(nameof(Pages.CreateFormPage), typeof(Pages.CreateFormPage));
            Routing.RegisterRoute(nameof(Pages.Forms), typeof(Pages.Forms));
            Routing.RegisterRoute(nameof(Pages.FormDataPage), typeof(Pages.FormDataPage));
            Routing.RegisterRoute(nameof(Pages.FillFormPage), typeof(Pages.FillFormPage));
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