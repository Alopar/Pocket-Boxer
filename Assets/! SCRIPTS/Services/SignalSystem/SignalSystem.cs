using System;
using System.Reflection;

namespace Services.SignalSystem
{
    public class SignalSystem : ISignalService
    {
        #region FIELDS PRIVATE
        private readonly EventBus _eventBus;
        #endregion

        #region CONSTRUCTORS
        public SignalSystem(EventBus eventBus)
        {
            _eventBus = eventBus;
        }
        #endregion

        #region METHODS PUBLIC
        public void Send<T>(T signal) where T : struct, ISignal
        {
            _eventBus.Send(signal);
        }

        public void AddListener<T>(Action<T> listener, bool instantNotify = false, int priority = 0) where T : struct, ISignal
        {
            _eventBus.AddListener(listener, instantNotify, priority);
        }

        public void RemoveListener<T>(Action<T> listener) where T : struct, ISignal
        {
            _eventBus.RemoveListener(listener);
        }

        /// <summary>
        /// Subscribe a listener for all events by attribute
        /// </summary>
        /// <param name="listener"></param>
        public void Subscribe(object listener)
        {
            ProcessingObject(listener, true);
        }

        /// <summary>
        /// Unsubscribe a listener for all events by attribute
        /// </summary>
        /// <param name="listener"></param>
        public void Unsubscribe(object listener)
        {
            ProcessingObject(listener, false);
        }
        #endregion

        #region METHODS PRIVATE
        private void ProcessingObject(object listener, bool isSubscribe)
        {
            var eventBusType = typeof(EventBus);
            var type = listener.GetType();

            var flags = BindingFlags.DeclaredOnly |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic;
            var methods = type.GetMethods(flags);

            foreach (var method in methods)
            {
                var attibutes = method.GetCustomAttributes(false);
                foreach (Attribute attibute in attibutes)
                {
                    if (!(attibute is SubscribeAttribute subscribeAttribute)) continue;

                    var parameters = method.GetParameters();
                    if (parameters.Length == 0) continue;

                    var parameter = parameters[0];
                    if (parameter.ParameterType.GetInterface(nameof(ISignal)) == null ) continue;

                    var delagateType = CreateGenericType(typeof(Action<>), parameter.ParameterType);
                    var delegat = method.CreateDelegate(delagateType, listener);

                    MethodInfo genericMethod;
                    object[] methodParameters;
                    if (isSubscribe)
                    {
                        methodParameters = new object[3] { delegat, subscribeAttribute.InstantNotify, subscribeAttribute.Priority };
                        genericMethod = eventBusType.GetMethod(nameof(_eventBus.AddListener)).MakeGenericMethod(parameter.ParameterType);
                    }
                    else
                    {
                        methodParameters = new object[1] { delegat };
                        genericMethod = eventBusType.GetMethod(nameof(_eventBus.RemoveListener)).MakeGenericMethod(parameter.ParameterType);
                    }

                    genericMethod?.Invoke(_eventBus, methodParameters);
                }
            }
        }

        private Type CreateGenericType(Type type, Type parameterType)
        {
            var typeArgs = new Type[1] { parameterType };
            var generic = type.MakeGenericType(typeArgs);

            return generic;
        }
        #endregion
    }
}
