using System;
using UnityEngine;

namespace Utility.DependencyInjection
{
    [DefaultExecutionOrder(-100)]
    public abstract class DependenciesContext : MonoBehaviour
    {
        protected DependenciesCollection _dependencies = new();
        private DependenciesProvider _provider;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            Setup();

            _provider = new(_dependencies);

            var children = GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var child in children)
            {
                _provider.Inject(child);
            }

            Configure();
        }

        protected abstract void Setup();

        protected abstract void Configure();
    }
}
