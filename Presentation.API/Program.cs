using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Presentation.API;

WebHost.CreateDefaultBuilder(args)
    .ConfigureServices(services => services.AddAutofac())
    .UseStartup<Startup>()
    .Build()
    .Run();
