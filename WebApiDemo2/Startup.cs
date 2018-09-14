using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApiDemo2.Extends;
using WebApiDemo2.Models;

namespace WebApiDemo2
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //请求注册的 Consul 地址
            ConsulClient consulClient = new ConsulClient(x =>
                x.Address = new Uri($"http://{Configuration["Consul:IP"]}:{Convert.ToInt32(Configuration["Consul:Port"])}"));
            services.AddSingleton(consulClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // register this service
            ServiceEntity serviceEntity = Configuration.GetSection("Service").Get<ServiceEntity>();
            serviceEntity.ConsulIP = Configuration["Consul:IP"];
            serviceEntity.ConsulPort = Convert.ToInt32(Configuration["Consul:Port"]);
            app.RegisterConsul(lifetime, serviceEntity);
        }
    }
}
