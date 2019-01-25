using System.Threading.Tasks;
using Wild.Pokenizer.Core.Models;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IPredictor
    {
        Task<PredictionResult> PredictAsync(string input);
    }
}
