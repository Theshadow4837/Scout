namespace Learning.Pages;

public class SubmissionDetailPage : ContentPage
{
    public SubmissionDetailPage(dynamic submission)
    {

        this.BackgroundColor = Color.FromRgb(12, 12, 12);

        Title = "Submission Detail";

        var layout = new VerticalStackLayout { Padding = 20, Spacing = 20 };

        
        layout.Children.Add(new Label
        {
            Text = submission.FormTitle,
            FontSize = 40,
            FontAttributes = FontAttributes.Bold
        });

        layout.Children.Add(new Label
        {
            Text = $"Submitted by: {submission.User}",
            TextColor = Colors.Gray
        });

        layout.Children.Add(new BoxView { HeightRequest = 2, Color = Colors.Green });

        
        if (submission.Answers != null)
        {
            int count = 1;
            foreach (var answer in submission.Answers)
            {
                var answerBox = new VerticalStackLayout
                {
                    Margin = new Thickness(0, 10),
                    Children = {
                        new Label { Text = $"Question {count}:", FontSize = 14, TextColor = Colors.LightBlue },
                        new Label { Text = answer.ToString(), FontSize = 18, Margin = new Thickness(5, 0) }
                    }
                };
                layout.Children.Add(answerBox);
                count++;
            }
        }

        Content = new ScrollView { Content = layout };
    }
}