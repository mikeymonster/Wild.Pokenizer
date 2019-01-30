using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Wild.Pokenizer.Core.Predictors;
using Xamarin.Forms;

namespace Wild.Pokenizer.Xamarin
{
    public partial class MainPage : ContentPage
    {
        private MediaFile _file;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void UploadPictureButton_Clicked(object sender, EventArgs e)
        {
            _file = null;

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No upload", "Picking a photo is not supported.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            _file = file;

            AppBusyIndicator.IsVisible = true;
            AppBusyIndicator.IsRunning = true;
            TargetImage.Source = ImageSource.FromStream(() => file.GetStream());
            AppBusyIndicator.IsRunning = false;
            AppBusyIndicator.IsVisible = false;
        }

        private async void TakePictureButton_Clicked(object sender, EventArgs e)
        {
            _file = null;

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.
                    IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                Name = "test.jpg"
            });
            if (file == null)
                return;

            _file = file;

            AppBusyIndicator.IsVisible = true;
            AppBusyIndicator.IsRunning = true;
            TargetImage.Source = ImageSource.FromStream(() => file.GetStream());
            AppBusyIndicator.IsRunning = false;
            AppBusyIndicator.IsVisible = false;
        }

        private async void PredictButton_OnClickedButton_Clicked(object sender, EventArgs e)
        {
            //TODO: Get a less hacky way of referencing the file
            if (_file == null)
            {
                return;
            }

            //TODO: Inject predictor

            var predictor = new DefaultPredictor();
            var prediction = await predictor.PredictAsync(_file.GetStream());
            BindingContext = prediction;
            AppBusyIndicator.IsRunning = false;
        }
    }
}
