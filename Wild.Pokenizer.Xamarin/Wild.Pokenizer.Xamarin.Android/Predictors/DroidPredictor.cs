using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.Models;
using Xamarin.TensorFlow.Lite;

namespace Wild.Pokenizer.Xamarin.Droid.Predictors
{
    public class DroidPredictor : IPredictor
    {
        public string Name => typeof(TensorFlowLite).AssemblyQualifiedName;

        public string Version
        {
            get
            {
                var ver = TensorFlowLite.Version();
                return TensorFlowLite.Version();
            }
        }

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
            //var tf = new Tensor();

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
