
using System.Collections.ObjectModel;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IMainViewModel
    {
        string Message { get; }

        ObservableCollection<Prediction> Predictions { get; }
        
        Prediction TopPrediction { get; }
    }
}
