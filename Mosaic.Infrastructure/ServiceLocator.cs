using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mosaic.Infrastructure
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance = new ServiceLocator();

        public IContainer Container { get;private set; }
        public ContainerBuilder Builder { get; }

        private ServiceLocator()
        {           
            this.Builder = new ContainerBuilder();
            Assembly Repository = Assembly.Load("Mosaic.Repositories");
            Assembly Domain = Assembly.Load("Mosaic.Domain");

            this.Builder.RegisterAssemblyTypes(Repository);
            this.Builder.RegisterAssemblyTypes(Domain, Repository).AsImplementedInterfaces().InstancePerLifetimeScope();

            this.Container = this.Builder.Build();
            
        }

        public T GetRef<T>()
        {
            try
            {
                return this.Container.Resolve<T>();
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
