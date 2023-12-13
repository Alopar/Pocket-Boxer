using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.ScreenSystem;

namespace Screens.Layers.Arena
{
    public class ArenaHUDLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _playerHpText;
        [SerializeField] private Slider _playerHpSlider;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _enemyHpText;
        [SerializeField] private Slider _enemyHpSlider;

        [Space(10)]
        [SerializeField] private AbilityButton _headButton;
        [SerializeField] private AbilityButton _blockButton;
        [SerializeField] private AbilityButton _dodgeButton;
        #endregion

        #region HANDLERS
        private void UseAbility(AbilityType type)
        {
            Debug.Log(type.ToString());
            switch (type)
            {
                case AbilityType.Block:
                    break;
                case AbilityType.Dodge:
                    break;
                case AbilityType.Headbutt:
                    break;
                case AbilityType.HandKick:
                    break;
                case AbilityType.FootKick:
                    break;
            }
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            SubscribeAbilityButtons();
        }

        protected override void OnDisable()
        {
            UnsubscribeAbilityButtons();
        }
        #endregion

        #region METHODS PRIVATE
        private string CreateTextLabel(int currentValue, int maxValue)
        {
            var digitCount = (int)Math.Log10(Math.Abs(maxValue)) + 1;
            var format = "{0:d" + digitCount + "}";
            return $"{string.Format(format, currentValue)}/{maxValue}";
        }

        private string ConvertNumberToText(uint number)
        {
            if (number < 1000) return number.ToString();
            if (number < 10000) return $"{((float)number / 1000):f2}K";
            if (number < 100000) return $"{((float)number / 1000):f1}K";
            if (number < 1000000) return $"{((float)number / 1000):f0}K";
            return $"{(float)number / 1000000:f2}M";
        }

        private void SubscribeAbilityButtons()
        {
            _blockButton.OnAbility += UseAbility;
            _headButton.OnAbility += UseAbility;
            _dodgeButton.OnAbility += UseAbility;
        }

        private void UnsubscribeAbilityButtons()
        {
            _blockButton.OnAbility -= UseAbility;
            _headButton.OnAbility -= UseAbility;
            _dodgeButton.OnAbility -= UseAbility;
        }

        private void ActivateButtonStates()
        {
            _blockButton.SetState(AbilityButtonState.Active);
            _headButton.SetState(AbilityButtonState.Active);
            _dodgeButton.SetState(AbilityButtonState.Active);
        }
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
            ActivateButtonStates();
        }
        #endregion
    }
}
