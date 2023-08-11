using System.Collections;
using System.Collections.Generic;

namespace Utility.DependencyInjection
{
    public class DependenciesCollection : IEnumerable<DependencyStruct>
    {
        private List<DependencyStruct> _dependencies = new();

        public void Add(DependencyStruct dependency)
        {
            _dependencies.Add(dependency);
        }

        public IEnumerator<DependencyStruct> GetEnumerator()
        {
            return _dependencies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dependencies.GetEnumerator();
        }
    }
}
