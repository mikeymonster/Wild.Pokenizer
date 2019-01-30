using System.Windows.Input;
using Xamarin.Forms;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;
using System;

//TODO: DELETE THIS! Just here to make it compile
namespace Xamarin.Forms
{
    internal class Command : ICommand
    {
        private Action _action;

        public Command(Action action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }

}
namespace Wild.Pokenizer.Core.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        #region Private members

        private ICommand _takePictureCommand;
        private ICommand _uploadPictureCommand;
        private ICommand _predictCommand;

        #endregion

        #region Commands
        
        public ICommand TakePictureCommand 
            => (_takePictureCommand ?? (_takePictureCommand = new Command(TakePicture)));

        public ICommand UploadPictureCommand
            => (_uploadPictureCommand ?? (_uploadPictureCommand = new Command(UploadPicture)));

        public ICommand PredictCommand 
            => (_predictCommand ?? (_predictCommand = new Command(Predict)));

        #endregion

        #region Preperties

        public bool IsBusy { get; }

        public string TargetImageSource { get; }

        public PredictionResult Prediction { get; }

        #endregion

        #region Constructors

        public MainViewModel()
        {

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
