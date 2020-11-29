using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Service.PublishScheduleDetection.DI
{
    public class AutofacCompositionRootModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var infrastructureData = AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == "Infrastructure");

            #region Repositories

            // scan the assembly for classes that end in "Repository" and add them to the DI container
            builder.RegisterAssemblyTypes(infrastructureData)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            #endregion

            #region Services

            // scan the assembly for classes that end in "Repository" and add them to the DI container
            builder.RegisterAssemblyTypes(infrastructureData)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            #endregion

            #region UseCases

            var core = AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == "Core");

            // scan the assembly for classes that end in "Repository" and add them to the DI container
            builder.RegisterAssemblyTypes(core)
               .Where(t => t.Name.EndsWith("UseCase"))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

            #endregion
        }
    }
}
