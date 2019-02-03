using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Xamarin.Droid.Predictors
{
    public class DroidPredictor : IPredictor
    {
        public async Task<PredictionResult> PredictAsync(string input)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from Android string."
                    });
        }

        public async Task<PredictionResult> PredictAsync(Stream stream)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from Android stream."
                    });
        }
    }
}
