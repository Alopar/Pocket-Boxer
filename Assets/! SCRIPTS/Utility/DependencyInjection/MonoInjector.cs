using System.Reflection;
using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public static class MonoInjector
    {
        public static object Inject(Component dependant)
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
                    var attibute = field.GetCustomAttribute<MonoInjectAttribute>(false);
                    if (attibute is null) continue;

                    var component = dependant.GetComponent(field.FieldType);
                    if(component is null) continue;

                    field.SetValue(dependant, component);
                }

                type = type.BaseType;
            }

            return dependant;
        }
    }
}
