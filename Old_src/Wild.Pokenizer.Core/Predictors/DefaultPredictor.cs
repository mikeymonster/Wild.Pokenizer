using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Core.Predictors
{
    public class DefaultPredictor : IPredictor
    {
        public async Task<PredictionResult> PredictAsync(string input)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from string.",
                        PredictionTime = DateTime.Now
                    });
        }

        public async Task<PredictionResult> PredictAsync(Stream stream)
        {
            return
                await Task.FromResult(
                    new PredictionResult
                    {
                        Success = true,
                        Answer = "Prediction from stream.",
                        PredictionTime = DateTime.Now
                    });
        }

        public string Name => nameof(DefaultPredictor);

        public string Version => 
            Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
