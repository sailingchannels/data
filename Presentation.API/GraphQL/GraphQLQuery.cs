using GraphQL.Types;
using Presentation.API.GraphQL.Resolver;
using System;
using System.Linq;

namespace Presentation.API.GraphQL
{
    public sealed class GraphQLQuery : ObjectGraphType
    {
        public GraphQLQuery(IServiceProvider serviceProvider)
        {
            var type = typeof(IResolver);

            // use reflection to find all resolvers within this current assembly, 
            // that implement the IResolver interface
            var resolversTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            // each query is handled by a dedicated resolver, we loop all available resolvers
            // and try to request an instantiation of that resolver from the DI services container
            // via IServiceProvider dependency
            foreach (Type resolverType in resolversTypes)
            {
                Type resolverTypeInterface = resolverType.GetInterfaces().Where(x => x != type).FirstOrDefault();
                if (resolverTypeInterface != null)
                {
                    // get the resolverTypeInterface implementation from the .NET Core DI services and map them as IResolver
                    // this way, we can generically call the IResolver.Resolve method on the automatically instantiated resolver class
                    var resolver = serviceProvider.GetService(resolverTypeInterface) as IResolver;
                    resolver.ResolveQuery(this);
                }
            }
        }
    }
}
