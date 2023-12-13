using System;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Layers.Arena
{
    public class AbilityButton : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Button _button;
        [SerializeField] private Image _background;

        [Space(10)]
        [SerializeField] private Color _disaleColor;
        [SerializeField] private Color _cooldownColor;
        [SerializeField] private Color _activeColor;

        [Space(10)]
        [SerializeField] private AbilityType _abilityType;
        #endregion

        #region PROPERTIES
        public AbilityType AbilityType => _abilityType;
        #endregion

        #region EVENTS
        public event Action<AbilityType> OnAbility;
        #endregion

        #region HANDLERS
        private void OnButtonClick()
        {
            OnAbility?.Invoke(_abilityType);
        }
        #endregion

        #region FIELDS PRIVATE
        private AbilityButtonState _currentState;
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

        #region METHODS PUBLIC
        public void SetState(AbilityButtonState state)
        {
            _currentState = state;
            switch (_currentState)
            {
                case AbilityButtonState.Disable:
                    _button.enabled = false;
                    _background.color = _disaleColor;
                    break;
                case AbilityButtonState.Cooldown:
                    _button.enabled = false;
                    _background.color = _cooldownColor;
                    break;
                case AbilityButtonState.Active:
                    _button.enabled = true;
                    _background.color = _activeColor;
                    break;
            }
        }

        public void SetCooldown(float value)
        {
            _background.fillAmount = value;
        }
        #endregion
    }
}
