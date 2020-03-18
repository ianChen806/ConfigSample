using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConfigSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var sectionConfig = new MySectionConfig();
            Configuration.Bind(sectionConfig);
            
            var rootConfig = new MyRootConfig();
            Configuration.GetSection("TestSection").Bind(rootConfig,
                                                         options =>
                                                         {
                                                             options.BindNonPublicProperties = true;
                                                         });

            services.AddSingleton(rootConfig);
            services.AddSingleton(sectionConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
        }
    }

    public class MySectionConfig
    {
        public LoggingConfig Logging { get; set; }

        public string AllowedHosts { get; set; }

        public MyRootConfig TestSection { get; set; }

        public class LoggingConfig
        {
        }
    }

    public class MyRootConfig
    {
        public int Id { get; set; }

        private string Name { get; set; }

        public MyRootConfig Child { get; set; }
    }
}