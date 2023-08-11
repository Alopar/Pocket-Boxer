using System;

namespace Utility.DependencyInjection
{
    public struct DependencyStruct
    {
        public Type Type;
        public bool IsSingleton;
        public DependencyFactory.Delegate Factory;
    }
}
