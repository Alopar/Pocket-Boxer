using UnityEngine;
using UnityEngine.UIElements;
using Utility.DependencyInjection;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class SceneEntryPoint : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Inject] private ComponentDependencyResolver _componentResolver;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            SelfResolver();
            RegisterDependencyContext();
        }

        private void Start()
        {
            ResolveDependency();
        }
        #endregion

        #region METHODS PRIVATE
        private void SelfResolver()
        {
            DependencyContainer.Inject(this);
        }

        private void RegisterDependencyContext()
        {
            DependencyContainer.Bind<PlayerFactory>();
            DependencyContainer.Bind<TokenFactory>();
        }

        private void ResolveDependency()
        {
            var objects = FindObjectsOfType<GameObject>();
            foreach (var go in objects)
            {
                var children = go.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var child in children)
                {
                    _componentResolver.Resolve(child);
                    DependencyContainer.Inject(child);
                }

                if(go.TryGetComponent<IActivatable>(out var component))
                {
                    component.Activate();
                }
            }
        }
        #endregion
    }
}
