using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Xamarin.UWP.Predictors
{
    public class UwpPredictor : IPredictor
    {
        public async Task<PredictionResult> PredictAsync(string input)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from UWP string."
                    });
        }

        public async Task<PredictionResult> PredictAsync(Stream stream)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from UWP stream."
                    });
        }
    }
}
