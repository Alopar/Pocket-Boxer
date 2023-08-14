using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class SceneEntryPoint : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            RegisterDependencyContext();
        }

        private void Start()
        {
            ResolveDependency();
        }
        #endregion

        #region METHODS PRIVATE
        private void RegisterDependencyContext()
        {
            DependencyContainer.Bind<PlayerFactory>();
        }

        private void ResolveDependency()
        {
            var objects = FindObjectsOfType<GameObject>();
            foreach (var go in objects)
            {
                var children = go.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var child in children)
                {
                    MonoInjector.Inject(child);
                    DependencyContainer.Inject(child);
                }
            }
        }
        #endregion
    }
}
