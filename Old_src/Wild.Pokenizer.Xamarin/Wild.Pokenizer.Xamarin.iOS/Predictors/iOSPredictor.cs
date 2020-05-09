using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Xamarin.iOS.Predictors
{
    // ReSharper disable once InconsistentNaming
    public class iOSPredictor : IPredictor
    {
        public iOSPredictor()
        {
        }

        public async Task<PredictionResult> PredictAsync(string input)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from iOS string."
                    });
        }

        public async Task<PredictionResult> PredictAsync(Stream stream)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from iOS stream."
                    });
        }
    }
}
