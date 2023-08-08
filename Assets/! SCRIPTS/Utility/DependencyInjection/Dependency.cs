using System;

namespace Utility.DependencyInjection
{
    public struct Dependency
    {
        public Type Type { get; set; }
        public bool IsSingleton { get; set; }
        public DependencyFactory.Delegate Factory { get; set; }
    }
}
