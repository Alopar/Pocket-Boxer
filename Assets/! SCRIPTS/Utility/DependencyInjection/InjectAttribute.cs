using System;

namespace Utility.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        private readonly string _id;
        public string ID => _id;
        public InjectAttribute(string id = null) => _id = id;
    }
}
