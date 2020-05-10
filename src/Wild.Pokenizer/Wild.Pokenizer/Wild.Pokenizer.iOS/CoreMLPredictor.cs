using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.iOS
{
    public class CoreMLPredictor : IPredictor
    {
        public Task<IEnumerable<Prediction>> PredictAsync(Stream input)
        {
            throw new NotImplementedException();
        }
    }
}