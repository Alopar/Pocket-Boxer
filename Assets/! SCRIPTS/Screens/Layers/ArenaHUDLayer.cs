using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.ScreenSystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Gameplay;

namespace Screens.Layers.Arena
{
    public class ArenaHUDLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _playerHpText;
        [SerializeField] private Slider _playerHpSlider;
        [SerializeField] private TextMeshProUGUI _playerNameText;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _enemyHpText;
        [SerializeField] private Slider _enemyHpSlider;
        [SerializeField] private TextMeshProUGUI _enemyNameText;
        #endregion

        #region FIELDS PRIVATE
        private BoxerController _playerBoxer;
        private BoxerController _opponentBoxer;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void BoxerSpawn(BoxerSpawn signal)
        {
            var boxer = signal.BoxerController;
            if (boxer.ControleType == ControleType.Player)
            {
                _playerBoxer = boxer;
                _playerNameText.text = boxer.Name;
            }
            else
            {
                _opponentBoxer = boxer;
                _enemyNameText.text = boxer.Name;
            }

            boxer.OnHealthChange += BoxerHealthChange;
        }

        private void BoxerHealthChange(ControleType controleType, int current, int max)
        {
            if(controleType == ControleType.Player)
            {
                _playerHpText.text = current.ToString();
                _playerHpSlider.value = (float)current / max;
            }
            else
            {
                _enemyHpText.text = current.ToString();
                _enemyHpSlider.value = (float)current / max;
            }
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _playerBoxer.OnHealthChange -= BoxerHealthChange;
            _opponentBoxer.OnHealthChange -= BoxerHealthChange;
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
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
        }
        #endregion
    }
}
