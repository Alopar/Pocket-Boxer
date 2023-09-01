using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public abstract class AbstractSceneEntryPoint : MonoBehaviour
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
            InitiateScene();
        }
        #endregion

        #region METHODS PRIVATE
        private void SelfResolver()
        {
            DependencyContainer.Inject(this);
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

        protected abstract void RegisterDependencyContext();

        protected abstract void InitiateScene();
        #endregion
    }
}
