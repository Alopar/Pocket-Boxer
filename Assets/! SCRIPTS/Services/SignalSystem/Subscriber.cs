using System;
using System.Reflection;

namespace Services.SignalSystem
{
    public class Subscriber : ISubscribeService
    {
        #region FIELDS PRIVATE
        private const string SUBSCRIBE_METHOD_NAME = "AddListener";
        private const string UNSUBSCRIBE_METHOD_NAME = "RemoveListener";
        #endregion

        #region METHODS PRIVATE
        private void ProcessingObject(object listener, bool isSubscribe)
        {
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
                    if (!(attibute is SubscribeAttribute eventHolderAttribute)) continue;

                    var parameters = method.GetParameters();
                    if (parameters.Length == 0) continue;
                    var parameter = parameters[0];

                    var delagateType = CreateGenericType(typeof(Action<>), parameter.ParameterType);
                    var delegat = method.CreateDelegate(delagateType, listener);

                    var eventHolder = CreateGenericType(typeof(SignalSystem<>), parameter.ParameterType);

                    object[] methodParameters;
                    MethodInfo eventHolderMethod;
                    if (isSubscribe)
                    {
                        methodParameters = new object[2] { delegat, eventHolderAttribute.InstantNotify };
                        eventHolderMethod = eventHolder.GetMethod(SUBSCRIBE_METHOD_NAME);
                    }
                    else
                    {
                        methodParameters = new object[1] { delegat };
                        eventHolderMethod = eventHolder.GetMethod(UNSUBSCRIBE_METHOD_NAME);
                    }

                    eventHolderMethod?.Invoke(null, methodParameters);
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

        #region METHODS PUBLIC
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
    }
}