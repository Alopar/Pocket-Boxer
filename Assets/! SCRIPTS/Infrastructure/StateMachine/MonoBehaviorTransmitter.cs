using UnityEngine;
using UnityEngine.Events;

namespace Utility.StateMachine
{
    public class MonoBehaviorTransmitter : MonoBehaviour
    {
        #region EVENTS
        public event UnityAction<UpdateType> CommonUpdate;
        public event UnityAction<ContactType, Collider> CommonTrigger;
        public event UnityAction<ContactType, Collision> CommonCollision;
        public event UnityAction MonoDestroy;
        #endregion

        #region UNITY CALLBACKS
        private void Update() => CommonUpdate?.Invoke(UpdateType.Update);
        private void FixedUpdate() => CommonUpdate?.Invoke(UpdateType.FixedUpdate);
        private void LateUpdate() => CommonUpdate?.Invoke(UpdateType.LateUpdate);

        private void OnTriggerEnter(Collider other) => CommonTrigger?.Invoke(ContactType.Enter, other);
        private void OnTriggerStay(Collider other) => CommonTrigger?.Invoke(ContactType.Stay, other);
        private void OnTriggerExit(Collider other) => CommonTrigger?.Invoke(ContactType.Exit, other);

        private void OnCollisionEnter(Collision collision) => CommonCollision?.Invoke(ContactType.Enter, collision);
        private void OnCollisionStay(Collision collision) => CommonCollision?.Invoke(ContactType.Stay, collision);
        private void OnCollisionExit(Collision collision) => CommonCollision?.Invoke(ContactType.Exit, collision);

        private void OnDestroy() => MonoDestroy?.Invoke();
        #endregion
    }

    public enum UpdateType
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

    public enum ContactType
    {
        Enter,
        Stay,
        Exit
    }
}
