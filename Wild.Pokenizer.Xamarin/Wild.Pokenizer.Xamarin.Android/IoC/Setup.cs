using Android.Content.Res;
using Autofac;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Xamarin.Droid.Predictors;
using Wild.Pokenizer.Xamarin.IoC;

namespace Wild.Pokenizer.Xamarin.Droid.IoC
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);
            
            cb.RegisterType<DroidPredictor>().As<IPredictor>();
            cb.RegisterType<DroidAssetLoader>().As<IAssetLoader>();
        }
    }
}