using UnityEngine;

namespace Utility.DependencyInjection
{
    public static class DependencyFactory
    {
        public delegate object Delegate(DependenciesProvider dependencies);

        public static Delegate FromClass<T>() where T : class, new()
        {
            return (dependencies) =>
            {
                return dependencies.Inject(new T());
            };
        }

        public static Delegate FromPrefab<T>(T prefab) where T : MonoBehaviour
        {
            return (dependencies) =>
            {
                var instance = GameObject.Instantiate(prefab);
                var children = instance.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var child in children)
                {
                    dependencies.Inject(child);
                }

                return instance.GetComponent<T>();
            };
        }

        public static Delegate FromGameObject<T>(T instance) where T : MonoBehaviour
        {
            return (dependencies) =>
            {
                var children = instance.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var child in children)
                {
                    dependencies.Inject(child);
                }

                return instance;
            };
        }
    }
}