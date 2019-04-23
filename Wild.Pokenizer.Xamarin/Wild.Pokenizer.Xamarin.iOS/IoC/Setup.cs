using Autofac;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Xamarin.iOS.Predictors;
using Wild.Pokenizer.Xamarin.IoC;

namespace Wild.Pokenizer.Xamarin.iOS.IoC
{
    public class Setup : AppSetup
    {
        protected override void RegisterDependencies(ContainerBuilder cb)
        {
            base.RegisterDependencies(cb);

            cb.RegisterType<iOSPredictor>().As<IPredictor>();
        }
    }
}