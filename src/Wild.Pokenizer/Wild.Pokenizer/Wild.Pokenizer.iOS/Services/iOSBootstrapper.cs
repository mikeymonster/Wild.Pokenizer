using Autofac;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.iOS.Services
{
    public class iOSBootstrapper : IBootstrapper
    {
        public iOSBootstrapper()
        {
        }

        public void Init(ContainerBuilder builder)
        {
            builder.RegisterType<CoreMLPredictor>().As<IPredictor>();
        }
    }
}
