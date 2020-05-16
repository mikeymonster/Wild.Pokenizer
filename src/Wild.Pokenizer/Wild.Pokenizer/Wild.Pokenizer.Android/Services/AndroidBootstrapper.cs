using Autofac;
using Wild.Pokenizer.Core.Interfaces;

namespace Wild.Pokenizer.Droid.Services
{
    public class AndroidBootstrapper : IBootstrapper
    {
        public void Init(ContainerBuilder builder)
        {
            builder.RegisterType<AndroidAssetLoader>().As<IAssetLoader>();
            builder.RegisterType<TensorflowLitePredictor>().As<IPredictor>();
        }
    }
}
