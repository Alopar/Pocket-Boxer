using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.SignalSystem
{
    public class EventBus
    {
        #region FIELDS PRIVATE
        private readonly Dictionary<Type, List<Receiver>> _receivers = new();
        private readonly Dictionary<Type, ISignal> _lastSignals = new();
        #endregion

        #region METHODS PUBLIC
        public void AddListener<T>(Action<T> listener, bool instantNotify = false, int priority = 0) where T : struct, ISignal
        {
            var type = typeof(T);
            if (!_receivers.ContainsKey(type))
            {
                _receivers[type] = new();
            }

            _receivers[type].Add(new(listener, priority));
            _receivers[type] = _receivers[type].OrderByDescending(e => e.Priority).ToList();

            if (instantNotify && _lastSignals.ContainsKey(type))
            {
                listener?.Invoke((T)_lastSignals[type]);
            }
        }

        public void RemoveListener<T>(Action<T> listener) where T : struct, ISignal
        {
            var type = typeof(T);
            if (!_receivers.ContainsKey(type)) return;

            var receiver = _receivers[type].FirstOrDefault(e => e.Listener.Equals(listener));
            if (receiver is null) return;

            _receivers[type].Remove(receiver);
        }

        public void Send<T>(T signal) where T : struct, ISignal
        {
            var type = typeof(T);
            _lastSignals[type] = signal;

            if (!_receivers.ContainsKey(type)) return;
            
            foreach (var receiver in _receivers[type])
            {
                var listener = receiver.Listener as Action<T>;
                listener?.Invoke(signal);
            }
        }
        #endregion
    }
}
