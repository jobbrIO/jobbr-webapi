using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Jobbr.ComponentModel.Registration;

namespace Jobbr.Server.WebAPI
{
    public class DependencyResolverAdapter : IDependencyResolver
    {
        private readonly IJobbrServiceProvider serviceProvider;


        public DependencyResolverAdapter(IJobbrServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.serviceProvider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new DependencyResolverAdapter(this.serviceProvider.GetChild());
        }
    }
}