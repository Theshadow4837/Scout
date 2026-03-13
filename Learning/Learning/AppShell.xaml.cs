using Microsoft.Extensions.DependencyInjection;
namespace Learning
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            

        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            
            Preferences.Remove("UserId");
            Preferences.Remove("MyTeamCode");
  
            await Shell.Current.GoToAsync($"//{nameof(Login)}");
        }

        private void MenuFlyoutItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}
