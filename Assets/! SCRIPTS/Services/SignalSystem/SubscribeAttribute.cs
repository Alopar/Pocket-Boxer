using System;

namespace Services.SignalSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeAttribute : Attribute
    {
        private readonly bool _instantNotify = true;
        private readonly int _priority = 0;

        public bool InstantNotify => _instantNotify;
        public int Priority => _priority;

        public SubscribeAttribute()
        {

        }

        public SubscribeAttribute(bool instantNotify)
        {
            _instantNotify = instantNotify;
        }

        public SubscribeAttribute(int priority)
        {
            _priority = priority;
        }

        public SubscribeAttribute(bool instantNotify, int priority)
        {
            _instantNotify = instantNotify;
            _priority = priority;
        }
    }
}