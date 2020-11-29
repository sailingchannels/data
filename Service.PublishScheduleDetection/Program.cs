using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.PublishScheduleDetection.DI;

namespace Service.PublishScheduleDetection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // di container
                    services.AddAutofac();

                    // database
                    services.AddMongoDB();

                    services.AddHostedService<Worker>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    // registering services in the Autofac ContainerBuilder
                    builder.RegisterModule(new AutofacCompositionRootModule());
                });
    }
}
