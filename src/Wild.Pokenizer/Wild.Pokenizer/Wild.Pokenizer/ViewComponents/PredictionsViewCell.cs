using Wild.Pokenizer.Core.Models;
using Xamarin.Forms;

namespace Wild.Pokenizer.ViewComponents
{
    public class PredictionsViewCell : ViewCell
    {
        public PredictionsViewCell()
        {
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            var titleLabel = new Label();
            titleLabel.SetBinding(Label.TextProperty, nameof(Prediction.Label));

            var predictionLabel = new Label();
            predictionLabel.SetBinding(Label.TextProperty, nameof(Prediction.Probability));

            layout.Children.Add(titleLabel);
            layout.Children.Add(predictionLabel);

            View = layout;
        }
    }
}
