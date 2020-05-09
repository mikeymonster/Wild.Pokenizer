using Autofac;
using Wild.Pokenizer.Core.Interfaces;
using Wild.Pokenizer.Core.ViewModels;
using Wild.Pokenizer.Xamarin.Providers;

namespace Wild.Pokenizer.Xamarin.IoC
{
    /// <summary>
    /// IoC Setup.
    /// Based on https://www.jamesalt.com/getting-started-with-autofac-and-xamarin-forms/.
    /// </summary>
    public class AppSetup
    {
        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<DisplayAlertProvider>()
                .As<IDisplayAlertProvider>();

            cb.RegisterType<MediaProvider>()
                .As<IMediaProvider>();

            cb.RegisterType<MainViewModel>()
                .As<IMainViewModel>()
                .SingleInstance();
        }
    }
}
