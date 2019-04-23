using Autofac;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Xamarin.IoC;
using Xamarin.Forms;

namespace Wild.Pokenizer.Xamarin
{
    public class ViewPage<T> : ContentPage where T : IViewModel
    {
        public T ViewModel { get; }

        public ViewPage()
        {
            using (AppContainer.Container.BeginLifetimeScope())
            {
                ViewModel = AppContainer.Container.Resolve<T>();
            }
            BindingContext = ViewModel;
        }
    }
}
