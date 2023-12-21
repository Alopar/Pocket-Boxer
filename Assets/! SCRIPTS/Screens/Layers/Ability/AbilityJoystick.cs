using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Screens.Layers.Arena
{
    public class AbilityJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        #region FIELDS INSPECTOR
        [SerializeField] private float _handleRange = 1;
        [SerializeField] private float _deadZone = 0;

        [SerializeField] protected RectTransform _footing = null;
        [SerializeField] private RectTransform _handle = null;

        [Space(10)]
        [SerializeField] private Image _filler;
        [SerializeField] private Image _handleBackground;
        [SerializeField] private Image _ghostBackground;

        [Space(10)]
        [SerializeField] private Color _disaleColor;
        [SerializeField] private Color _cooldownColor;
        [SerializeField] private Color _activeColor;

        [Space(10)]
        [SerializeField] private AbilityType _abilityType;
        #endregion

        #region FIELDS PRIVATE
        private Canvas _canvas;
        private Camera _camera;

        private Vector2 _input = Vector2.zero;

        private AbilityButtonState _currentState;
        private bool _enabled = false;
        private bool _isOn = false;
        #endregion

        #region PROPERTIES
        public Vector2 Direction => _input;
        public AbilityType AbilityType => _abilityType;
        #endregion

        #region EVENTS
        public event Action<Vector2> OnInput;
        public event Action<AbilityType, TargetZone> OnAbility;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("The Joystick is not placed inside a canvas");
            }

            SetupHandle();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_enabled)
            {
                DropJoystick();
                return;
            }

            SetCamera();

            var position = RectTransformUtility.WorldToScreenPoint(_camera, _footing.position);
            var radius = _footing.sizeDelta / 2;

            _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
            _input = _input.magnitude > 1 ? _input.normalized : _input;

            _handle.anchoredPosition = _input * radius * _handleRange;

            OnInput?.Invoke(Direction);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!_enabled) return;

            OnDrag(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!_enabled)
            {
                DropJoystick();
                return;
            }

            var zone = TargetZone.None;
            if (_input.y > 0.5f)
            {
                zone = TargetZone.Top;
            }
            else if (_input.y < 0.5 && _input.y > -0.5f)
            {
                zone = TargetZone.Middle;
            }
            else
            {
                zone = TargetZone.Bottom;
            }

            OnAbility.Invoke(_abilityType, zone);
            DropJoystick();
        }
        #endregion

        #region METHODS PRIVATE
        private void SetCamera()
        {
            _camera = null;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                _camera = _canvas.worldCamera;
            }
        }

        private void SetupHandle()
        {
            var center = new Vector2(0.5f, 0.5f);
            _footing.pivot = center;
            _handle.anchorMin = center;
            _handle.anchorMax = center;
            _handle.pivot = center;
            _handle.anchoredPosition = Vector2.zero;

            _filler.fillAmount = 1f;
        }

        private void DropJoystick()
        {
            _input = Vector2.zero;
            _handle.anchoredPosition = Vector2.zero;
        }

        private void SetColorBackground(Color color)
        {
            _filler.color = color;
            _ghostBackground.color = color;

            color.a = 0.6f;
            _handleBackground.color = color;
        }
        #endregion

        #region METHODS PUBLIC
        public void TurnOn()
        {
            _isOn = true;
            _enabled = true;
            SetState(_currentState);
        }

        public void TurnOff()
        {
            _isOn = false;
            _enabled = false;
            SetColorBackground(_disaleColor);
        }

        public void SetState(AbilityButtonState state)
        {
            _currentState = state;

            var enable = false;
            var stateColor = Color.white;
            switch (_currentState)
            {
                case AbilityButtonState.Disable:
                    stateColor = _disaleColor;
                    break;
                case AbilityButtonState.Cooldown:
                    stateColor = _cooldownColor;
                    break;
                case AbilityButtonState.Active:
                    enable = true;
                    stateColor = _activeColor;
                    break;
            }

            if (_isOn)
            {
                _enabled = enable;
                SetColorBackground(stateColor);
            }
        }

        public void SetCooldown(float value)
        {
            _filler.fillAmount = value;
        }
        #endregion
    }
}
