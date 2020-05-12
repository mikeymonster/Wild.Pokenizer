
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using TinyMvvm;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Core.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IMediaProvider _mediaProvider;
        private readonly IPredictor _predictor;

        private string _message;
        private ObservableCollection<Prediction> _predictions;

        public string Message
        {
            get => _message;
            private set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged(nameof(Message));
                }
            }
        }

        public ObservableCollection<Prediction> Predictions
        {
            get => _predictions;
            set
            {
                if (_predictions != value)
                {
                    _predictions = value;
                    RaisePropertyChanged(nameof(Predictions));
                    RaisePropertyChanged(nameof(TopPrediction));
                }
            }
        }

        public Prediction TopPrediction => Predictions.FirstOrDefault();

        public MainViewModel(IMediaProvider mediaProvider, IPredictor predictor)
        {
            _predictor = predictor;
            _mediaProvider = mediaProvider;
            Message = "";
            Predictions = new ObservableCollection<Prediction>();
        }

        // ReSharper disable once RedundantOverriddenMember
        public override async Task Initialize()
        {
            await base.Initialize();
        }

        public ICommand PickPhotoCommand =>
            new TinyCommand(async () => await PickPhotoAsync());

        public ICommand TakePhotoCommand =>
            new TinyCommand(async () => await TakePhotoAsync());

        private async Task PickPhotoAsync()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file != null)
            {
                await ProcessPhoto(file);
            }
        }

        private async Task TakePhotoAsync()
        {
            //await _mediaProvider.Media.Initialize();
            //if (!_mediaProvider.Media.IsCameraAvailable
            //    || !_mediaProvider.Media.IsTakePhotoSupported)
            //{
            //    await _displayAlertProvider.DisplayAlert("No Camera", "No camera available.", "Ok");
            //    return;
            //}

            var file = await _mediaProvider.Media.TakePhotoAsync(new StoreCameraMediaOptions
            {
                CompressionQuality = 50,
                CustomPhotoSize = 50
                //SaveToAlbum = true,
                //Name = "test.jpg"
            });

            if (file != null)
            {
                //TargetImageSource = file.Path;//ImageSource.FromStream(() => file.GetStream());
                //await PredictAsync(file.GetStream());
                await ProcessPhoto(file);
            }
        }

        private async Task ProcessPhoto(MediaFile file)
        {
            IsBusy = true;

            var stream = file.GetStreamWithImageRotatedForExternalStorage();

            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();

            //_predictor.PredictionCompleted += Predictor_PredictionCompleted;

            var predictions = await _predictor.PredictAsync(stream);
            Predictions.Clear();
            foreach (var prediction in predictions)
            {
                Predictions.Add(prediction);
            }
            RaisePropertyChanged(nameof(TopPrediction));

            IsBusy = false;
        }
    }
}
