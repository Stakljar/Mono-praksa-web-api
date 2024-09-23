using Autofac;
using Introduction.Repository.Common;

namespace Introduction.Repository
{
    public class RepositoryDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CatRepository>().As<ICatRepository>().InstancePerDependency();
            builder.RegisterType<CatShelterRepository>().As<ICatShelterRepository>().InstancePerDependency();
        }
    }
}
