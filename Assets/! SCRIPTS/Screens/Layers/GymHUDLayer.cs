using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.CurrencySystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Services.ScreenSystem;
using Utility.DependencyInjection;

namespace Screens.Layers
{
    public class GymHUDLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _moneyText;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private Slider _strengthFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _dexterityText;
        [SerializeField] private Slider _dexterityFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _enduranceText;
        [SerializeField] private Slider _enduranceFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private Slider _energyFiller;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ICurrencyService _currencyService;
        #endregion

        #region HANDLERS
        private void CurrencyChanged(CurrencyType type, ulong value)
        {
            switch (type)
            {
                case CurrencyType.Money:
                    _moneyText.text = value.ToString();
                    break;
            }
        }

        [Subscribe]
        private void StrengthChange(StrengthChange info)
        {
            _strengthText.text = info.Level.ToString();
            _strengthFiller.value = info.Delta;
        }

        [Subscribe]
        private void DexterityChange(DexterityChange info)
        {
            _dexterityText.text = info.Level.ToString();
            _dexterityFiller.value = info.Delta;
        }

        [Subscribe]
        private void EnduranceChange(EnduranceChange info)
        {
            _enduranceText.text = info.Level.ToString();
            _enduranceFiller.value = info.Delta;
        }

        [Subscribe]
        private void BatteryOccupied(BatteryOccupied info)
        {
            _energyText.text = CreateTextLabel(info.Occupied, info.Capacity);

            var delta = (float)info.Occupied / info.Capacity;
            _energyFiller.value = delta;
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();
            _currencyService.OnCurrencyChanged += CurrencyChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _currencyService.OnCurrencyChanged -= CurrencyChanged;
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

            _moneyText.text = _currencyService.GetAmount(CurrencyType.Money).ToString();
        }
        #endregion
    }
}
