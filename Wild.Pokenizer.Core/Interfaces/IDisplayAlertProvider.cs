using System.Threading.Tasks;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IDisplayAlertProvider
    {
        Task DisplayAlert(string title, string message, string cancel);

        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
    }
}