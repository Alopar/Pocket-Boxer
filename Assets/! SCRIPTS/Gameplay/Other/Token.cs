using UnityEngine;
using Utility.MonoPool;
using Utility.DependencyInjection;

namespace Gameplay
{
    [SelectionBase]
    public class Token : MonoBehaviour, IMagnetable, IPoolable
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _view;
        [SerializeField, Range(0, 10)] private float _attractDelay;
        [SerializeField] private CurrencyType _currencyType;
        #endregion

        #region FIELDS PRIVATE
        [Find] private Collider _collider;
        [Find] private FloatComponent _floatComponent;

        private bool _isAttactable;
        private uint _cost;
        #endregion

        #region PROPERTIES
        public uint Cost => _cost;
        public CurrencyType Currency => _currencyType;
        #endregion

        #region METHODS PRIVATE
        private void PhysicsOn()
        {
            _collider.enabled = true;
            _floatComponent.TurnOn();
        }

        private void PhysicsOff()
        {
            _collider.enabled = false;
            _floatComponent.TurnOff();
        }

        private void AttractableOn()
        {
            _isAttactable = true;
        }
        #endregion

        #region METHODS PUBLIC
        public void Init(uint cost)
        {
            _cost = cost;
            PhysicsOn();

            _isAttactable = false;
            Invoke(nameof(AttractableOn), _attractDelay);
        }

        public bool TryAttract()
        {
            if (!_isAttactable) return false;
            PhysicsOff();

            return true;
        }

        public void Delete()
        {
            MonoPool.Return(this);
        }
        #endregion
    }
}
