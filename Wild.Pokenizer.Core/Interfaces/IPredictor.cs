using System.IO;
using System.Threading.Tasks;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IPredictor
    {
        string Name { get; }

        string Version { get; }

        Task<PredictionResult> PredictAsync(string input);

        Task<PredictionResult> PredictAsync(Stream input);
    }
}
