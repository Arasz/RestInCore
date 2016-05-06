using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using RESTService.Database;
using RESTService.Models;
using RESTService.Providers;
using RESTService.Repository;
using RESTService.Services;
using RESTService.Utils;
using System;

namespace RESTService
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container
        // We can change default IoC container for our custom container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            var mvc = services.AddMvc(configuration =>
           {
               configuration.RespectBrowserAcceptHeader = true;
               configuration.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
               configuration.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
           });

            mvc.AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSingleton<MongoDbManager>();
            services.AddSingleton<IRepository<Student>, StudentsRepository>();
            services.AddSingleton<IIdentityProvider<Student>, UniqueIdentityProvider<Student>>();
            services.AddSingleton<IIdentityAssignService<Student>, IdentityAssignService<Student>>();

            services.AddSingleton<IRepository<Subject>, SubjectsRepository>();
            services.AddSingleton<IIdentityProvider<Subject>, UniqueIdentityProvider<Subject>>();
            services.AddSingleton<IIdentityAssignService<Subject>, IdentityAssignService<Subject>>();

            services.AddSingleton<IIdentityProvider<Mark>, UniqueIdentityProvider<Mark>>();
            services.AddSingleton<IIdentityAssignService<Mark>, IdentityAssignService<Mark>>();

            services.AddSingleton<DataInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}