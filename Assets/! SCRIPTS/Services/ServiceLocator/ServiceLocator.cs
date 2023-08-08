using System;
using System.Collections.Generic;

namespace Services.ServiceLocator
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> _services = new();

        public static void RegisterService<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public static T GetService<T>() where T : class
        {
            return _services[typeof(T)] as T;
        }
    }
}
