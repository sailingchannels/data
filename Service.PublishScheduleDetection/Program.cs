using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.PublishScheduleDetection;
using Service.PublishScheduleDetection.DI;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // di container
        services.AddAutofac();

        // database
        services.AddMongoDb();

        services.AddHostedService<Worker>();
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        // registering services in the Autofac ContainerBuilder
        builder.RegisterModule(new AutofacCompositionRootModule());
    })
    .Build()
    .Run();