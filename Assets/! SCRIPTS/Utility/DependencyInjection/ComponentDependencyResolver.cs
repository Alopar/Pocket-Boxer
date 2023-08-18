using System.Reflection;
using UnityEngine;

namespace Utility.DependencyInjection
{
    public class ComponentDependencyResolver
    {
        public object Resolve(Component dependant)
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
                    var attibute = field.GetCustomAttribute<FindAttribute>(false);
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
