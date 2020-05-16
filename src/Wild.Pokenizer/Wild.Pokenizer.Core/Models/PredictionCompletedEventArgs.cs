using System;
using System.Collections.Generic;

namespace Wild.Pokenizer.Core.Models
{
    public class PredictionCompletedEventArgs : EventArgs
    {
        public List<Prediction> Predictions { get; private set; }

        public PredictionCompletedEventArgs(List<Prediction> predictions)
        {
            Predictions = predictions;
        }
    }
}
