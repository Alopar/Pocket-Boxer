using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventHolder;
using Services.ScreenSystem;

namespace Gameplay
{
    public class HUDLayer : AbstractScreen
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

        #region HANDLERS
        [EventHolder]
        private void MoneyChange(MoneyChangeInfo info)
        {
            _moneyText.text = info.Value.ToString();
        }

        [EventHolder]
        private void DiamondChange(DiamondChangeInfo info)
        {
            _diamondText.text = info.Value.ToString();
        }

        [EventHolder]
        private void ExperiencePointsChange(ExperiencePointsChangeInfo info)
        {
            _experienceText.text = info.Value.ToString();
        }

        [EventHolder]
        private void StrengthChange(StrengthChangeInfo info)
        {
            _strengthText.text = info.Level.ToString();
            _strengthFiller.fillAmount = info.Delta;
        }

        [EventHolder]
        private void DexterityChange(DexterityChangeInfo info)
        {
            _dexterityText.text = info.Level.ToString();
            _dexterityFiller.fillAmount = info.Delta;
        }

        [EventHolder]
        private void EnduranceChange(EnduranceChangeInfo info)
        {
            _enduranceText.text = info.Level.ToString();
            _enduranceFiller.fillAmount = info.Delta;
        }

        [EventHolder]
        private void BatteryOccupied(BatteryOccupiedInfo info)
        {
            _energyText.text = CreateTextLabel(info.Occupied, info.Capacity);

            var delta = (float)info.Occupied / info.Capacity;
            _energyFiller.fillAmount = delta;
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            SubscribeService.SubscribeListener(this);
        }

        private void OnDisable()
        {
            SubscribeService.UnsubscribeListener(this);
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
