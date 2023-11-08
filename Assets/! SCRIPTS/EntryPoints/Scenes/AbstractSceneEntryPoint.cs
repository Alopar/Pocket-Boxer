using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utility.DependencyInjection;
using NaughtyAttributes;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public abstract class AbstractSceneEntryPoint : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<MonoBehaviour> _dependants;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ComponentResolver _componentResolver;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            SelfResolver();
            RegisterDependencyContext();
            ResolveDependency();
        }

        private void Start()
        {
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
            foreach (var dependant in _dependants)
            {
                var children = dependant.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var child in children)
                {
                    DependencyContainer.Inject(child);
                    _componentResolver.Resolve(child);
                }
            }
        }

        protected abstract void RegisterDependencyContext();

        protected abstract void InitiateScene();
        #endregion

        #region METHODS PUBLIC
        [Button("FIND DEPENDANTS")]
        public void FindDependants()
        {
            _dependants = FindObjectsOfType<MonoBehaviour>().Where(e => e is IDependant).ToList();
        }
        #endregion
    }
}
