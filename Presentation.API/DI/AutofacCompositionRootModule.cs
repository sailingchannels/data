using Autofac;
using GraphQL;
using GraphQL.Types;
using Presentation.API.GraphQL;
using Presentation.API.GraphQL.Resolver;
using Presentation.API.GraphQL.Types;
using System;
using System.Linq;
using System.Reflection;

namespace Presentation.API.DI
{
    public class AutofacCompositionRootModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region GraphQL

            builder.RegisterType<DocumentExecuter>().As<IDocumentExecuter>();
            builder.RegisterType<GraphQlSchema>().As<ISchema>();
            builder.RegisterType<GraphQlQuery>();
            builder.RegisterType<GraphQlMutation>();
            builder.Register<Func<Type, GraphType>>(c =>
            {
                var context = c.Resolve<IComponentContext>();
                return t => {
                    var res = context.Resolve(t);
                    return (GraphType) res;
                };
            });

            // resolvers
            var resolverAssembly = typeof(IResolver).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(resolverAssembly)
                .Where(t => t.Name.EndsWith("Resolver"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // types
            var typesAssembly = typeof(IGraphQLType).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(typesAssembly)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsAssignableFrom(typeof(IGraphQLType))))
                .AsSelf();

            #endregion

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
