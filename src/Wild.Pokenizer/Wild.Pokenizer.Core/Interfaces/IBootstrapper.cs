using Autofac;

namespace Wild.Pokenizer.Core.Interfaces
{
    public interface IBootstrapper
    {
        void Init(ContainerBuilder builder);
    }
}
