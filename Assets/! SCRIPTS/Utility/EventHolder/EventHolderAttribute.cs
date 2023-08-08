using System;

namespace EventHolder
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHolderAttribute : Attribute
    {
        private readonly bool _instantNotify = true;
        public bool InstantNotify => _instantNotify;

        public EventHolderAttribute()
        {

        }

        public EventHolderAttribute(bool instantNotify)
        {
            _instantNotify = instantNotify;
        }
    }
}