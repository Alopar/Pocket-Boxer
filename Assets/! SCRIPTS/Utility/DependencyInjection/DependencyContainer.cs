using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Utility.DependencyInjection
{
    public static class DependencyContainer
    {
        #region FIELDS PRIVATE
        private static List<AbstractDependency> _dependencies = new();
        #endregion

        #region METHODS PRIVATE
        private static MethodInfo CreateGenericMethod(Type parameterType, string name, Type[] argumentTypes)
        {
            var type = typeof(DependencyContainer);
            var flags = BindingFlags.Static | BindingFlags.Public;
            var method = type.GetMethod(name, flags, null, argumentTypes, null);
            var typeArgs = new Type[1] { parameterType };
            return method.MakeGenericMethod(typeArgs);
        }
        #endregion

        #region METHODS PUBLIC
        public static Dependency<T> Bind<T>()
        {
            Dependency<T> dependency = null;
            var dependencies = _dependencies.OfType<Dependency<T>>().ToList();
            if(dependencies.Count == 0)
            {
                dependency = new Dependency<T>();
            }
            else
            {
                dependency = dependencies.Find(e => e.ID is null);
                if(dependency is not null)
                {
                    _dependencies.Remove(dependency);
                    dependency = new Dependency<T>();
                }
            }

            _dependencies.Add(dependency);
            return dependency;
        }

        public static Dependency<T> Bind<T>(string id)
        {
            Dependency<T> dependency = null;
            var dependencies = _dependencies.OfType<Dependency<T>>().ToList();
            if (dependencies.Count == 0)
            {
                dependency = new Dependency<T>(id);
            }
            else
            {
                dependency = dependencies.Find(e => e.ID == id);
                if (dependency is not null)
                {
                    _dependencies.Remove(dependency);
                }

                dependency = new Dependency<T>(id);
            }

            _dependencies.Add(dependency);
            return dependency;
        }

        public static T Get<T>()
        {
            Dependency<T> dependency = null;
            var dependencies = _dependencies.OfType<Dependency<T>>().ToList();
            if (dependencies.Count == 0)
            {
                throw new ArgumentException("Type is not a dependecy: " + typeof(T).FullName);
            }

            dependency = dependencies.Find(e => e.ID is null);
            if (dependency is null)
            {
                throw new ArgumentException("Type is not a dependecy: " + typeof(T).FullName);
            }

            return dependency.Instance;
        }

        public static T Get<T>(string id)
        {
            Dependency<T> dependency = null;
            var dependencies = _dependencies.OfType<Dependency<T>>().ToList();
            if (dependencies.Count == 0)
            {
                throw new ArgumentException("Type is not a dependecy: " + typeof(T).FullName);
            }

            dependency = dependencies.Find(e => e.ID == id);
            if (dependency is null)
            {
                throw new ArgumentException("Type is not a dependecy: " + typeof(T).FullName);
            }

            return dependency.Instance;
        }

        public static object Inject(object dependant)
        {
            var flags = BindingFlags.DeclaredOnly
                | BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic;

            var type = dependant.GetType();
            while (type is not null)
            {
                var fields = type.GetFields(flags);
                foreach (var field in fields)
                {
                    var attibute = field.GetCustomAttribute<InjectAttribute>(false);
                    if (attibute is null) continue;

                    var argumentTypes = attibute.ID is null ? Type.EmptyTypes : new Type[1] { typeof(string) };
                    var method = CreateGenericMethod(field.FieldType, nameof(Get), argumentTypes);
                    var parameters = attibute.ID is null ? null : new object[1] { attibute.ID };
                    field.SetValue(dependant, method.Invoke(null, parameters));
                }

                type = type.BaseType;
            }

            return dependant;
        }
        #endregion
    }
}
