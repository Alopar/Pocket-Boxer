using System;
using System.Runtime.Serialization;

namespace Utility.DependencyInjection
{
    public class Dependency<TContract>
    {
        #region FIELDS PRIVATE
        private Type _instanceType;
        private TContract _instance;
        private bool _isSingleton = false;
        private string _id = "";
        #endregion

        #region CONSTRUCTORS
        public Dependency()
        {
            _instanceType = typeof(TContract);
        }
        #endregion

        #region PROPERTIES
        public TContract Instance
        {
            get
            {
                if (_isSingleton)
                {
                    return _instance ??= CreateInstance(_instanceType);
                }

                return CreateInstance(_instanceType);
            }
        }
        #endregion

        #region METHODS PRIVATE
        private TContract CreateInstance(Type type)
        {
            var obj = FormatterServices.GetUninitializedObject(type);
            type.GetConstructor(Type.EmptyTypes).Invoke(obj, null);
            return (TContract)obj;
        }
        #endregion

        #region METHODS PUBLIC
        public Dependency<TContract> To<TInstance>() where TInstance : TContract, new()
        {
            _instanceType = typeof(TInstance);
            return this;
        }

        public Dependency<TContract> AsSingle()
        {
            _isSingleton = true;
            return this;
        }

        public Dependency<TContract> FromInstance(TContract instance)
        {
            _instanceType = instance.GetType();
            _instance = instance;
            _isSingleton = true;
            return this;
        }
        #endregion
    }
}
