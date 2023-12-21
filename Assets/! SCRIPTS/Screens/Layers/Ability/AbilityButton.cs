using System;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Layers.Arena
{
    public class AbilityButton : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Button _button;

        [Space(10)]
        [SerializeField] private Image _filler;
        [SerializeField] private Image _background;

        [Space(10)]
        [SerializeField] private Color _disaleColor;
        [SerializeField] private Color _cooldownColor;
        [SerializeField] private Color _activeColor;

        [Space(10)]
        [SerializeField] private AbilityType _abilityType;
        #endregion

        #region FIELDS PRIVATE
        private bool _isOn = false;
        private AbilityButtonState _currentState;
        #endregion

        #region PROPERTIES
        public AbilityType AbilityType => _abilityType;
        #endregion

        #region EVENTS
        public event Action<AbilityType, TargetZone> OnAbility;
        #endregion

        #region HANDLERS
        private void OnButtonClick()
        {
            OnAbility?.Invoke(_abilityType, TargetZone.None);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
        #endregion

        #region METHODS PRIVATE
        private void SetColorBackground(Color color)
        {
            _filler.color = color;

            color.a = 0.6f;
            _background.color = color;
        }
        #endregion

        #region METHODS PUBLIC
        public void TurnOn()
        {
            _isOn = true;
            SetState(_currentState);
        }

        public void TurnOff()
        {
            _isOn = false;
            _button.enabled = false;
            SetColorBackground(_disaleColor);
        }

        public void SetState(AbilityButtonState state)
        {
            var enable = false;
            var color = Color.white;
            var fill = 0f;
            _currentState = state;
            switch (_currentState)
            {
                case AbilityButtonState.Disable:
                    enable = false;
                    color = _disaleColor;
                    fill = 1f;
                    break;
                case AbilityButtonState.Cooldown:
                    enable = false;
                    color = _cooldownColor;
                    fill = 0f;
                    break;
                case AbilityButtonState.Active:
                    enable = true;
                    color = _activeColor;
                    fill = 1f;
                    break;
            }

            if (_isOn)
            {
                _button.enabled = enable;
                SetColorBackground(color);
            }

            _filler.fillAmount = fill;
        }

        public void SetCooldown(float value)
        {
            _filler.fillAmount = value;
        }
        #endregion
    }
}
