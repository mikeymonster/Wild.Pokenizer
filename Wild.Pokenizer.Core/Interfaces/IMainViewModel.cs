using Wild.Pokenizer.Core.Models;
using System.Windows.Input;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IMainViewModel : IViewModel
    {
        ICommand TakePictureCommand { get; }

        ICommand UploadPictureCommand { get; }

        bool IsBusy { get; }
        //https://stackoverflow.com/questions/43343966/binding-image-source-dynamically-on-xamarin-forms

        string TargetImageSource { get; }

        PredictionResult Prediction { get;  }

        string PredictorName { get; }

        string PredictorVersion { get; }
    }
}
