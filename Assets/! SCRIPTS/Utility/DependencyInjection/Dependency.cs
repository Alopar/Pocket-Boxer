using System;
using System.Runtime.Serialization;

namespace Utility.DependencyInjection
{
    public class Dependency<TContract> : AbstractDependency
    {
        #region FIELDS PRIVATE
        private string _id = null;
        private Type _instanceType;
        private TContract _instance;
        private bool _isSingleton = false;
        #endregion

        #region CONSTRUCTORS
        public Dependency()
        {
            _instanceType = typeof(TContract);
        }

        public Dependency(string id)
        {
            _id = id;
            _instanceType = typeof(TContract);
        }
        #endregion

        #region PROPERTIES
        public string ID => _id;
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
            DependencyContainer.Inject(obj);
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
            DependencyContainer.Inject(instance);
            _instanceType = instance.GetType();
            _instance = instance;
            _isSingleton = true;
            return this;
        }

        public Dependency<TContract> NonLazy()
        {
            _instance = CreateInstance(_instanceType);
            return this;
        }
        #endregion
    }
}
