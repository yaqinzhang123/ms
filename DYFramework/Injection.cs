using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DYFramework
{
    public static class Injection
    {

        public static IServiceCollection RegisterAssembly(this IServiceCollection services,string assemblyName)
        {
            return RegisterAssembly(services, Assembly.Load(assemblyName));
        }
        public static IServiceCollection RegisterAssembly(this IServiceCollection services,Assembly assembly)
        {
            var types = assembly.GetTypes().Where(p => p.IsGenericType);
            foreach(var type in types)
            {
                services.AddScoped(type);
            }
            return services;
        }
        public static IServiceCollection RegisterAssembly(this IServiceCollection services,string interfaceAssemblyName,string implAssemblyName)
        {
            Assembly interfaces = Assembly.Load(interfaceAssemblyName);
            Assembly implAssembly = Assembly.Load(implAssemblyName);
            return RegisterAssembly(services, interfaces, implAssembly);
        }
        public static IServiceCollection RegisterAssembly(this IServiceCollection services,Assembly interfaces,Assembly implAssembly)
        {
            var domainInterfaces = interfaces.GetTypes().Where(p => p.IsInterface);
            foreach (Type type in domainInterfaces)
            {
                var types = implAssembly.GetTypes().Where(p => p.GetInterfaces().Contains(type));
                foreach (Type impl in types)
                {
                    services.AddScoped(type, impl);
                }
            }
            return services;
        }
    }
}
