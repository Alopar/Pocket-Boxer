using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using AYellowpaper;

namespace Gameplay
{
    public class FloorButton : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private InterfaceReference<IInteractable> _interactableObject;

        [Space(10)]
        [SerializeField] private GameObject _view;
        [SerializeField] private BoxCollider _collider;
        #endregion

        #region FIELDS PRIVATE
        private bool _isActivated = false;
        #endregion

        #region PROPERTIES
        public bool IsActivated => _isActivated;
        public float InteractTime => _interactableObject.Value.InteractTime;
        #endregion

        #region EVENTS
        public event UnityAction OnPushButton;
        #endregion

        #region HANDLERS
        private void ReadyHandler()
        {
            _collider.enabled = true;
            _view.gameObject.SetActive(true);
        }

        private void BusyHandler()
        {
            _collider.enabled = false;
            _view.gameObject.SetActive(false);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _collider.enabled = false;
            _view.gameObject.SetActive(false);
        }

        private void OnEnable()
        {   
            _interactableObject.Value.OnReady += ReadyHandler;
            _interactableObject.Value.OnBusy += BusyHandler;
        }

        private void OnDisable()
        {
            _interactableObject.Value.OnReady -= ReadyHandler;
            _interactableObject.Value.OnBusy -= BusyHandler;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.Tag() != Tag.Player) return;

            _isActivated = false;
        }
        #endregion

        #region METHODS PUBLIC
        public void PushButton()
        {
            if (_isActivated) return;

            _isActivated = true;
            OnPushButton?.Invoke();
        }
        #endregion
    }

    public interface IInteractable
    {
        public event UnityAction OnReady;
        public event UnityAction OnBusy;
        public float InteractTime { get; }
    }
}