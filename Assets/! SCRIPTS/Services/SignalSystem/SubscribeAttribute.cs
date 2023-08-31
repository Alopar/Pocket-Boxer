using System;

namespace Services.SignalSystem
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeAttribute : Attribute
    {
        private readonly bool _instantNotify = true;
        public bool InstantNotify => _instantNotify;

        public SubscribeAttribute()
        {

        }

        public SubscribeAttribute(bool instantNotify)
        {
            _instantNotify = instantNotify;
        }
    }
}