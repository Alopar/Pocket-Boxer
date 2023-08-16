using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventHolder;
using Services.ScreenSystem;

namespace Gameplay
{
    //TODO: create number formatter method
    public class HudUiController : AbstractScreenController
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
            var money = info.Value;
            //_moneyText.text = money < 1000 ? money.ToString() : money < 1000000 ? $"{(float)money / 1000}K" : $"{(float)money / 1000000}ÊÊ";
            _moneyText.text = money.ToString();
        }

        [EventHolder]
        private void DiamondChange(DiamondChangeInfo info)
        {
            var money = info.Value;
            //_diamondText.text = money < 1000 ? money.ToString() : money < 1000000 ? $"{(float)money / 1000}K" : $"{(float)money / 1000000}ÊÊ";
            _diamondText.text = money.ToString();
        }

        [EventHolder]
        private void ExperiencePointsChange(ExperiencePointsChangeInfo info)
        {
            var value = info.Value;
            //_diamondText.text = money < 1000 ? money.ToString() : money < 1000000 ? $"{(float)money / 1000}K" : $"{(float)money / 1000000}ÊÊ";
            _experienceText.text = value.ToString();
        }

        [EventHolder]
        private void BatteryOccupied(BatteryOccupiedInfo info)
        {
            _energyText.text = CreateTextLabel(info.Occupied, info.Capacity);

            var delta = (float)info.Occupied / info.Capacity;
            _energyFiller.fillAmount = delta;
        }

        [EventHolder]
        //TODO: fix only one call
        private void StatsChange(StatsChangeInfo info)
        {
            switch (info.Type)
            {
                case StatType.Strength:
                    _strengthText.text = info.Level.ToString();
                    _strengthFiller.fillAmount = info.Delta;
                    break;
                case StatType.Dexterity:
                    _dexterityText.text = info.Level.ToString();
                    _dexterityFiller.fillAmount = info.Delta;
                    break;
                case StatType.Endurance:
                    _enduranceText.text = info.Level.ToString();
                    _enduranceFiller.fillAmount = info.Delta;
                    break;
            }
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
        #endregion
    }
}