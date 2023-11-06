using System;

namespace Services.SignalSystem
{
    public interface ISignalService
    {
        void Send<T>(T signal) where T : struct, ISignal;
        void AddListener<T>(Action<T> listener, bool instantNotify = false, int priority = 0) where T : struct, ISignal;
        void RemoveListener<T>(Action<T> listener) where T : struct, ISignal;

        void Subscribe(object listener);
        void Unsubscribe(object listener);
    }
}