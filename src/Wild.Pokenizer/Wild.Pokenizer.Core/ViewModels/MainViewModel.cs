
using TinyMvvm;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.Core.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public string Message { get; private set;  }
        private readonly IPredictor _predictor;

        public MainViewModel(IPredictor predictor)
        {
            _predictor = predictor;
            Message = "Hello world!";
        }
    }
}
