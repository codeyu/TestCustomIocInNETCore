using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TestCustomIocInNETCore
{
    internal class ServiceScope : IServiceScope
    {
        private MyServiceProvider _serviceProvider;
        public ServiceScope(IServiceScope innserServiceScope, Dictionary<Type, Type> services)
        {
            _serviceProvider = new MyServiceProvider(innserServiceScope.ServiceProvider, services);
        }
        public IServiceProvider ServiceProvider => _serviceProvider;

        public void Dispose()
        {
            _serviceProvider.Dispose();
        }
    }
}