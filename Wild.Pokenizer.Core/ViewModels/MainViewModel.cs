using System.Windows.Input;
using Xamarin.Forms;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Core.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        #region Private members

        private readonly IMediaProvider _mediaProvider;
        private readonly IPredictor _predictor;

        private ICommand _takePictureCommand;
        private ICommand _uploadPictureCommand;
        private ICommand _predictCommand;

        #endregion

        #region Commands
        
        public ICommand TakePictureCommand 
            => (_takePictureCommand 
                ?? (_takePictureCommand 
                    = new Command(TakePicture,
                        () => !IsBusy)));

        public ICommand UploadPictureCommand
            => (_uploadPictureCommand 
                ?? (_uploadPictureCommand 
                    = new Command(UploadPicture,
                        () => !IsBusy)));

        public ICommand PredictCommand 
            => (_predictCommand 
                ?? (_predictCommand 
                    = new Command(Predict,
                        () => !IsBusy)));

        #endregion

        #region Properties

        public bool IsBusy { get; }

        public string TargetImageSource { get; }

        public PredictionResult Prediction { get; }

        #endregion

        #region Constructors

        
        public MainViewModel(IMediaProvider mediaProvider, IPredictor predictor)
        {
            _predictor = predictor;
            _mediaProvider = mediaProvider;
        }

        #endregion

        #region Private methods

        private void TakePicture()
        {

        }

        private void UploadPicture()
        {

        }
        
        private void Predict()
        {

        }

        #endregion

    }
}
