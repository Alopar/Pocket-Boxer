using System;

namespace Utility.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        private readonly string _id = "";
        public string ID => _id;

        public InjectAttribute()
        {

        }

        public InjectAttribute(string id)
        {
            _id = id;
        }
    }
}