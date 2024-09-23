using Autofac;
using Introduction.Service.Common;

namespace Introduction.Service
{
    public class ServiceDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CatService>().As<ICatService>().InstancePerDependency();
            builder.RegisterType<CatShelterService>().As<ICatShelterService>().InstancePerDependency();
        }
    }
}
