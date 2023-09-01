using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.SignalSystem;
using Services.ScreenSystem;

namespace Gameplay
{
    public class ArenaHUDLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private Image _strengthFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _dexterityText;
        [SerializeField] private Image _dexterityFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _enduranceText;
        [SerializeField] private Image _enduranceFiller;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void ShowScreen(ShowScreenInfo info)
        {
            if (info.ScreenType != ScreenType.ArenaHUD) return;

            ShowScreen();
            SignalSystem<ScreenOpenedInfo>.Send(new(ScreenType.ArenaHUD));
        }

        [Subscribe]
        private void CloseScreen(CloseScreenInfo info)
        {
            if (info.ScreenType != ScreenType.ArenaHUD) return;
            CloseScreen();
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            HideScreen();
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
    }
}
