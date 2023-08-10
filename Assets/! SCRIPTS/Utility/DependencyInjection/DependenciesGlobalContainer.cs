using System;
using UnityEngine;

namespace Utility.DependencyInjection
{
    public static class DependenciesGlobalContainer
    {
        private static DependenciesCollection _dependencies;
        private static DependenciesProvider _provider;

        public static DependenciesProvider Provider => _provider;

        public static void Init(DependenciesCollection dependencies)
        {
            _provider = new(_dependencies);
        }
    }
}
