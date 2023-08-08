using UnityEngine;
using Utility.MonoPool;

namespace Gameplay
{
    [SelectionBase]
    public class Cargo : MonoBehaviour, IMagnetable, IPoolable
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _view;

        [SerializeField, Range(0, 10)] private float _attractDelay;
        #endregion

        #region FIELDS PRIVATE
        private Collider _collider;
        private Rigidbody _rigidbody;

        private bool _isAttactable;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            ResolveDependency();
        }

        private void ResolveDependency()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void PhysicsOn()
        {
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
        }

        private void PhysicsOff()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        private void SetForce(Vector3 force, Vector3 torque)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
            _rigidbody.AddTorque(torque, ForceMode.Impulse);
        }

        private void AttractableOn()
        {
            _isAttactable = true;
        }
        #endregion

        #region METHODS PUBLIC
        public void GiveKick(Vector3 force, Vector3 torque)
        {
            PhysicsOn();
            SetForce(force, torque);

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