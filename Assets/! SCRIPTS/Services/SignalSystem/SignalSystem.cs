using System;
using System.Linq;
using System.Collections.Generic;

namespace Services.SignalSystem
{
    public static class SignalSystem<T> where T : class
    {
        #region FIELDS PRIVATE
        private static readonly List<Action<T>> _listeners = new List<Action<T>>();

        private static T _currentSignal;
        #endregion

        #region METHODS PUBLIC
        public static void Send(T signal)
        {
            _currentSignal = signal;
            var currentListeners = _listeners.ToList();
            foreach (var listener in currentListeners)
            {
                listener?.Invoke(signal);
            }
        }

        public static void AddListener(Action<T> listener, bool instantNotify)
        {
            _listeners.Add(listener);

            if (instantNotify && _currentSignal != null)
            {
                listener?.Invoke(_currentSignal);
            }
        }

        public static void RemoveListener(Action<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }
        #endregion
    }
}
