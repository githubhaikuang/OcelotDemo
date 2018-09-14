using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApiDemo2.Models;

namespace WebApiDemo2.Extends
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime,
            ServiceEntity serviceEntity)
        {
            var consulClient = app.ApplicationServices.GetService<ConsulClient>();
            var httpCheck = new AgentServiceCheck
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), //服务启动多久后注册
                Interval = TimeSpan.FromSeconds(serviceEntity.Check.Interval), //健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{serviceEntity.IP}:{serviceEntity.Port}{serviceEntity.Check.Path}", //健康检查地址
                Timeout = TimeSpan.FromSeconds(serviceEntity.Check.Timeout)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration
            {
                Checks = new[] {httpCheck},
                ID = Guid.NewGuid().ToString(),
                Name = serviceEntity.Name,
                Address = serviceEntity.IP,
                Port = serviceEntity.Port,
                Tags = new[] {$"urlprefix-/{serviceEntity.Name}"} //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait(); //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait(); //服务停止时取消注册
            });

            return app;
        }
    }
}
