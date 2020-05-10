using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Droid
{
    public class TensorflowLitePredictor : IPredictor
    {
        public async Task<IEnumerable<Prediction>> PredictAsync(Stream input)
        {
            return new List<Prediction>();
        }
    }
}