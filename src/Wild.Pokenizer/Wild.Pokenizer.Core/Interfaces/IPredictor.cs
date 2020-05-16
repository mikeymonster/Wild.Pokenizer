using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IPredictor
    {
        event EventHandler<PredictionCompletedEventArgs> PredictionCompleted;

        Task<IEnumerable<Prediction>> PredictAsync(Stream input);
    }
}
