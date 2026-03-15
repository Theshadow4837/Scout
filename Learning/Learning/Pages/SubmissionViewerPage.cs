using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace Learning.Pages;

public class SubmissionViewerPage : ContentPage
{
    private List<object> _submissions;
    private int _index = 0;
    private VerticalStackLayout _layout;

    public SubmissionViewerPage(List<object> submissions)
    {
        _submissions = submissions;
        BackgroundColor = Colors.Black;
        Title = "Reviewing Submissions";

        _layout = new VerticalStackLayout { Padding = 20, Spacing = 10 };
        Content = new ScrollView { Content = _layout };

        ShowData();
    }

    private void ShowData()
    {
        _layout.Children.Clear();
        try
        {
            if (_submissions == null || _index >= _submissions.Count) return;

            var rawJson = JsonConvert.SerializeObject(_submissions[_index]);
            var jObj = Newtonsoft.Json.Linq.JObject.Parse(rawJson);

            // Based on your screenshot, the data is inside "Object"
            var content = jObj["Object"];

            // 1. Map to your EXACT keys from the screenshot
            string title = content["formtitle"]?.ToString() ?? "Untitled Form";
            string user = content["user"]?.ToString() ?? "Anonymous User";
            string time = content["createdat"]?.ToString() ?? "No Date";

            // 2. Build the Header
            _layout.Children.Add(new Label
            {
                Text = title,
                FontSize = 28,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            });
            _layout.Children.Add(new Label { Text = $"By: {user}", TextColor = Colors.LightGreen, HorizontalOptions = LayoutOptions.Center });
            _layout.Children.Add(new Label { Text = $"Sent: {time}", TextColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center });

            _layout.Children.Add(new BoxView { HeightRequest = 1, Color = Colors.White, Margin = 15 });

            // 3. Map Answers (Your screenshot shows they are in the "questions" array)
            var answersToken = content["questions"];
            if (answersToken != null && answersToken.HasValues)
            {
                foreach (var ans in answersToken)
                {
                    _layout.Children.Add(new Frame
                    {
                        BackgroundColor = Color.FromArgb("#222222"),
                        BorderColor = Color.FromArgb("#444444"),
                        Margin = new Thickness(0, 5),
                        Content = new Label { Text = ans.ToString(), TextColor = Colors.White, FontSize = 16 }
                    });
                }
            }
            else
            {
                _layout.Children.Add(new Label { Text = "No answers found.", TextColor = Colors.DarkGray, HorizontalOptions = LayoutOptions.Center });
            }

            // 4. Navigation Buttons
            var nav = new HorizontalStackLayout { Spacing = 10, HorizontalOptions = LayoutOptions.Center, Margin = 20 };

            if (_index > 0)
            {
                var prev = new Button { Text = "Previous" };
                prev.Clicked += (s, e) => { _index--; ShowData(); };
                nav.Children.Add(prev);
            }

            if (_index < _submissions.Count - 1)
            {
                var nxt = new Button { Text = "Next" };
                nxt.Clicked += (s, e) => { _index++; ShowData(); };
                nav.Children.Add(nxt);
            }

            var del = new Button { Text = "Delete", BackgroundColor = Colors.DarkRed };
            del.Clicked += OnDeleteClicked;
            nav.Children.Add(del);

            _layout.Children.Add(nav);

            // Show progress (e.g. 1 of 5)
            _layout.Children.Add(new Label
            {
                Text = $"{_index + 1} of {_submissions.Count}",
                TextColor = Colors.DimGray,
                HorizontalOptions = LayoutOptions.Center
            });
        }
        catch (Exception ex)
        {
            _layout.Children.Add(new Label { Text = "Display Error: " + ex.Message, TextColor = Colors.Red });
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm", "Delete this record?", "Yes", "No");
        if (!confirm) return;

        try
        {
            var rawJson = JsonConvert.SerializeObject(_submissions[_index]);
            var firebaseWrapper = JsonConvert.DeserializeObject<dynamic>(rawJson);
            string key = firebaseWrapper.Key; // Get the Firebase ID

            string teamCode = Preferences.Get("MyTeamCode", "");
            var client = new FirebaseClient("https://test-3b247-default-rtdb.firebaseio.com/");

            await client.Child("teams").Child(teamCode).Child("submissions").Child(key).DeleteAsync();

            await DisplayAlert("Success", "Deleted!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}