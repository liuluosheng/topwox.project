using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.IServices;
using Core.Repository;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WebService.Core.Authorization;
using Module = Autofac.Module;

namespace WebService.Core.Config
{
    public class AutofacModule
    {
        /// <summary>
        /// 注册公共的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceProvider Register(IServiceCollection services, IConfiguration configuration)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            builder.RegisterInstance(configuration).SingleInstance();
            builder.RegisterType<BaseRepository>().AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(BaseService<>)).As(typeof(IBaseService<>));
            builder.RegisterType<PermissionAuthorizationHandler>().As<IAuthorizationHandler>();
            //创建容器
            var Container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(Container);
        }
    }

}
