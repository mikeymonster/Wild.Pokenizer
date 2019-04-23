using Autofac;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Xamarin.IoC;
using Wild.Pokenizer.Xamarin.UWP.Predictors;

namespace Wild.Pokenizer.Xamarin.UWP.IoC
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);

            cb.RegisterType<UwpPredictor>().As<IPredictor>();
        }
    }
}