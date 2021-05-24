using OrderSolution.Infrastructure.Storage.Abstractions;
using OrderSolution.Infrastructure.Storage.Azure.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderSupervisor.Domain.DependencyInjection;
using System;

namespace OrderSupervisor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAzureAsStorageService(Configuration.GetSection("AzureConnection"), "confirmations");

            services.AddAzureQueueSetup();

            services.AddDomainServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Order Supervisor",
                    Version = "v1",
                    Description = "EndPoint For Supervisor Orders.",
                    Contact = new OpenApiContact
                    {
                        Name = "Media Valet Team",
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStorageEnvironmentConfiguration storageEnvironmentConfiguration)
        {
            storageEnvironmentConfiguration.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Supervisor v1"));
        }
    }
}
