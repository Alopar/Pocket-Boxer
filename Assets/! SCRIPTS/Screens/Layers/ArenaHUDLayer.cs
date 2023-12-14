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
        #endregion

        #region HANDLERS
        #endregion

        #region UNITY CALLBACKS
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
