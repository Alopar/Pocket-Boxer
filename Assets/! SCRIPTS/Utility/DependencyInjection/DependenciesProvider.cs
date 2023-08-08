using System;
using System.Reflection;
using System.Collections.Generic;

namespace Utility.DependencyInjection
{
    public class DependenciesProvider
    {
        private Dictionary<Type, Dependency> _dependencies = new();
        private Dictionary<Type, object> _singletons = new();

        public DependenciesProvider(DependenciesCollection dependencies)
        {
            foreach (var dependency in dependencies)
            {
                _dependencies.Add(dependency.Type, dependency);
            }
        }

        public object Get(Type type)
        {
            if (!_dependencies.ContainsKey(type))
            {
                throw new ArgumentException("Type is not a dependecy: " + type.FullName);
            }

            var dependency = _dependencies[type];
            if (dependency.IsSingleton)
            {
                if (!_singletons.ContainsKey(type))
                {
                    _singletons.Add(type, dependency.Factory(this));
                }

                return _singletons[type];
            }
            else
            {
                return dependency.Factory(this);
            }
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public object Inject(object dependant)
        {
            var type = dependant.GetType();
            while(type is not null)
            {
                var bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var fields = type.GetFields(bindingFlags);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<InjectFieldAttribute>(false) is null) continue;

                    field.SetValue(dependant, Get(field.FieldType));
                }

                type = type.BaseType;
            }

            return dependant;
        }
    }
}
