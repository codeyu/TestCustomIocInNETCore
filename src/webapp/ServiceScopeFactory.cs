using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TestCustomIocInNETCore
{
    internal class ServiceScopeFactory : IServiceScopeFactory
    {
        private IServiceScopeFactory _innerServiceFactory;
        private Dictionary<Type, Type> _services;
        public ServiceScopeFactory(IServiceScopeFactory innerServiceFactory, Dictionary<Type, Type> services)
        {
            _innerServiceFactory = innerServiceFactory;
            _services = services;
        }
        public IServiceScope CreateScope()
        {
            return new ServiceScope(_innerServiceFactory.CreateScope(), _services);
        }
    }
}