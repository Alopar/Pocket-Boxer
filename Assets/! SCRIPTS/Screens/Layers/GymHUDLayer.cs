using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.CurrencySystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Services.ScreenSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class GymHUDLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _diamondText;
        [SerializeField] private TextMeshProUGUI _experienceText;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private Image _strengthFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _dexterityText;
        [SerializeField] private Image _dexterityFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _enduranceText;
        [SerializeField] private Image _enduranceFiller;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI _energyText;
        [SerializeField] private Image _energyFiller;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ICurrencyService _currencyService;
        #endregion

        #region HANDLERS
        private void OnCurrencyChanged(CurrencyType type, ulong value)
        {
            switch (type)
            {
                case CurrencyType.Money:
                    _moneyText.text = value.ToString();
                    break;
                case CurrencyType.Diamond:
                    _diamondText.text = value.ToString();
                    break;
                case CurrencyType.ExperiencePoints:
                    _experienceText.text = value.ToString();
                    break;
            }
        }

        [Subscribe]
        private void ShowScreen(ShowScreen info)
        {
            if (info.ScreenType != ScreenType.GymHUD) return;

            ShowScreen();
            _signalService.Send<ScreenOpened>(new(ScreenType.GymHUD));
        }

        [Subscribe]
        private void CloseScreen(CloseScreen info)
        {
            if (info.ScreenType != ScreenType.GymHUD) return;
            CloseScreen();
        }

        [Subscribe]
        private void StrengthChange(StrengthChange info)
        {
            _strengthText.text = info.Level.ToString();
            _strengthFiller.fillAmount = info.Delta;
        }

        [Subscribe]
        private void DexterityChange(DexterityChange info)
        {
            _dexterityText.text = info.Level.ToString();
            _dexterityFiller.fillAmount = info.Delta;
        }

        [Subscribe]
        private void EnduranceChange(EnduranceChange info)
        {
            _enduranceText.text = info.Level.ToString();
            _enduranceFiller.fillAmount = info.Delta;
        }

        [Subscribe]
        private void BatteryOccupied(BatteryOccupied info)
        {
            _energyText.text = CreateTextLabel(info.Occupied, info.Capacity);

            var delta = (float)info.Occupied / info.Capacity;
            _energyFiller.fillAmount = delta;
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            HideScreen();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _currencyService.OnCurrencyChanged += OnCurrencyChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _currencyService.OnCurrencyChanged -= OnCurrencyChanged;
        }
        #endregion

        #region METHODS PRIVATE
        protected override void ShowScreen()
        {
            base.ShowScreen();

            _moneyText.text = _currencyService.GetAmount(CurrencyType.Money).ToString();
            _diamondText.text = _currencyService.GetAmount(CurrencyType.Diamond).ToString();
            _experienceText.text = _currencyService.GetAmount(CurrencyType.ExperiencePoints).ToString();
        }

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
