using System;
using System.Windows.Input;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Wild.Pokenizer.Core.ViewModels
{
    public class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        #region Private members

        private readonly IDisplayAlertProvider _displayAlertProvider;
        private readonly IMediaProvider _mediaProvider;
        private readonly IPredictor _predictor;

        private bool _isBusy;
        private string _targetImageSource;
        private PredictionResult _prediction;

        private ICommand _takePictureCommand;
        private ICommand _uploadPictureCommand;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Commands

        public ICommand TakePictureCommand
            => (_takePictureCommand
                ?? (_takePictureCommand
                    = new Command(
                        async () => await TakePictureAsync(),
                        () => !IsBusy)));

        public ICommand UploadPictureCommand
            => (_uploadPictureCommand
                ?? (_uploadPictureCommand
                    = new Command(
                        async () => await UploadPictureAsync(),
                        () => !IsBusy)));

        #endregion

        #region Properties

        public bool IsBusy
        {
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
            get => _isBusy;
        }

        public string TargetImageSource
        {
            set
            {
                if ((_targetImageSource) != value)
                {
                    _targetImageSource = value;
                    OnPropertyChanged();
                }
            }
            get => _targetImageSource;
        }

        public PredictionResult Prediction
        {
            set
            {
                if (_prediction != value)
                {
                    _prediction = value;
                    OnPropertyChanged();
                }
            }
            get => _prediction;
        }

        public string PredictorName =>
            _predictor.Name;

        public string PredictorVersion =>
            _predictor.Version;

        #endregion

        #region Constructors

        public MainViewModel(IDisplayAlertProvider displayAlertProvider, IMediaProvider mediaProvider, IPredictor predictor)
        {
            _displayAlertProvider = displayAlertProvider;
            _predictor = predictor;
            _mediaProvider = mediaProvider;

            Prediction = new PredictionResult
            {
                Answer = "INITIALIZED",
                Success = false,
                PredictionTime = DateTime.Now
            };
        }

        #endregion

        #region Private methods

        private async Task TakePictureAsync()
        {
            await _mediaProvider.Media.Initialize();
            if (!_mediaProvider.Media.IsCameraAvailable
                || !_mediaProvider.Media.IsTakePhotoSupported)
            {
                await _displayAlertProvider.DisplayAlert("No Camera", "No camera available.", "Ok");
                return;
            }

            var file = await _mediaProvider.Media.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                Name = "test.jpg"
            });
            if (file == null)
                return;

            IsBusy = true;
            TargetImageSource = file.Path;//ImageSource.FromStream(() => file.GetStream());
            await PredictAsync(file.GetStream());
            IsBusy = false;
        }

        private async Task UploadPictureAsync()
        {
            if (!_mediaProvider.Media.IsPickPhotoSupported)
            {
                await _displayAlertProvider.DisplayAlert("No upload", "Picking a photo is not supported.", "Ok");
                return;
            }

            var file = await _mediaProvider.Media.PickPhotoAsync();
            if (file == null)
            {
                return;
            }

            IsBusy = true;
            TargetImageSource = file.Path;
            await PredictAsync(file.GetStream());
            IsBusy = false;
        }

        private async Task PredictAsync(Stream stream)
        {
            var prediction = await _predictor.PredictAsync(stream);
            Prediction = prediction;
        }

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
