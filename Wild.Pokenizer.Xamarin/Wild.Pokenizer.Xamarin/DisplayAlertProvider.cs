using System.Threading.Tasks;
using Xamarin.Forms;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.Xamarin
{
    public class DisplayAlertProvider : IDisplayAlertProvider
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
