using System;
using System.Collections.Generic;

namespace GameLogic.Scripts.Decoupling
{
    public class ServiceLocator
    {
        #region Private Constructors

        private ServiceLocator()
        {
            _services = new Dictionary<Type, object>();
        }

        #endregion Private Constructors

        #region Public Properties

        public static ServiceLocator Instance => _instance ??= new ServiceLocator();

        #endregion Public Properties

        #region Private Fields

        private static ServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new();

        #endregion Private Fields

        #region Public Methods

        public bool Contains<Type>()
        {
            var serviceType = typeof(Type);
            return _services.ContainsKey(serviceType);
        }

        public Type GetService<Type>()
        {
            var serviceType = typeof(Type);
            if (!_services.TryGetValue(serviceType, out var service))
                throw new Exception($"Service of type {serviceType} not found");
            return (Type)service;
        }

        public void RegisterService<Type>(Type service)
        {
            var serviceType = typeof(Type);
            if (!_services.TryGetValue(serviceType, out var serviceToAdd))
                _services.Add(serviceType, service);
            else
                throw new Exception($"Service {service} is already registered in ServiceLocator");
        }

        public void UnregisterService<Type>()
        {
            var serviceType = typeof(Type);
            if (_services.TryGetValue(serviceType, out var serviceToAdd))
                _services.Remove(serviceType);
            else
                throw new Exception($"Service {serviceType} is not registered in ServiceLocator");
        }

        public void UnregisterAll()
        {
            var keys = new List<Type>(_services.Keys);

            for (int i=0; i< keys.Count; i++)
            {
                var serviceType = keys[i];
                _services.Remove(serviceType);
            }
        }

        #endregion Public Methods
    }
}