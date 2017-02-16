using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TestCustomIocInNETCore
{
    public class MyServiceProvider : IServiceProvider, IDisposable
    {
        private IServiceProvider        _innerServiceProvider;
        private Dictionary<Type, Type>  _services;
        private List<IDisposable>       _disposables;
        public MyServiceProvider(IServiceProvider innerServiceProvider)
        {
            _innerServiceProvider  = innerServiceProvider;
            this._services         = new Dictionary<Type, Type>();
            _disposables           = new List<IDisposable>();
        }
        public MyServiceProvider(IServiceProvider innerServiceProvider, Dictionary<Type, Type> services)
        {
            _innerServiceProvider  = innerServiceProvider;
            this._services         = services;
            _disposables           = new List<IDisposable>();
        }
        public MyServiceProvider Register<TFrom, TTo>() where TTo: TFrom, new()
        {
            _services[typeof(TFrom)] = typeof(TTo);
            return this;
        }
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IServiceScopeFactory))
            {
                IServiceScopeFactory innerServiceScopeFactory = _innerServiceProvider.GetRequiredService<IServiceScopeFactory>();
                return new ServiceScopeFactory(innerServiceScopeFactory, _services);
            }
            Type implementation;
            if (_services.TryGetValue(serviceType, out implementation))
            {
                object service = Activator.CreateInstance(implementation);
                IDisposable disposbale = service as IDisposable;
                if (null != disposbale)
                {
                    _disposables.Add(disposbale);
                }
                return service;
            }
            return _innerServiceProvider.GetService(serviceType);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    (_innerServiceProvider as IDisposable)?.Dispose();
                    foreach (var it in _disposables)
                    {
                        it.Dispose();
                    }
                    _disposables.Clear();
                }
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MyServiceProvider() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}