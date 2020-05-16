using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.iOS.Services
{
    public class CoreMLPredictor : IPredictor
    {
        public event EventHandler<PredictionCompletedEventArgs> PredictionCompleted;

        public Task<IEnumerable<Prediction>> PredictAsync(Stream input)
        {
            throw new NotImplementedException();
        }
    }
}