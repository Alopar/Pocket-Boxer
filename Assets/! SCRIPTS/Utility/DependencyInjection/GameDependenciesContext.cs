using System;
using System.Reflection;
using System.Collections.Generic;

namespace Utility.DependencyInjection
{
    public static class GameDependenciesContext
    {
        #region FIELDS PRIVATE
        private static Dictionary<Type, object> _dependencies = new();
        #endregion

        #region METHODS PRIVATE
        private static MethodInfo CreateGenericMethod(Type parameterType)
        {
            var type = typeof(GameDependenciesContext);

            var flags = BindingFlags.Static | BindingFlags.Public;
            var method = type.GetMethod(nameof(Get), flags);
            var typeArgs = new Type[1] { parameterType };
            return method.MakeGenericMethod(typeArgs);
        }
        #endregion

        #region METHODS PUBLIC
        public static Dependency<T> Bind<T>()
        {
            var dependency = new Dependency<T>();
            _dependencies[typeof(T)] = dependency;
            
            return dependency;
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (!_dependencies.ContainsKey(type))
            {
                throw new ArgumentException("Type is not a dependecy: " + type.FullName);
            }

            var dependency = _dependencies[type] as Dependency<T>;
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
                    if (field.GetCustomAttribute<InjectAttribute>(false) is null) continue;

                    var method = CreateGenericMethod(field.FieldType);
                    field.SetValue(dependant, method.Invoke(null, null));
                }

                type = type.BaseType;
            }

            return dependant;
        }
        #endregion
    }
}
