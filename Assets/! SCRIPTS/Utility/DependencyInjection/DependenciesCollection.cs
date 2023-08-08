using System.Collections;
using System.Collections.Generic;

namespace Utility.DependencyInjection
{
    public class DependenciesCollection : IEnumerable<Dependency>
    {
        private List<Dependency> _dependencies = new();

        public void Add(Dependency dependency)
        {
            _dependencies.Add(dependency);
        }

        public IEnumerator<Dependency> GetEnumerator()
        {
            return _dependencies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dependencies.GetEnumerator();
        }
    }
}
