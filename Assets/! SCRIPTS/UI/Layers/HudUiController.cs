using UnityEngine;
using TMPro;
using EventHolder;
using System;

namespace Gameplay
{
    public class HudUiController : AScreenUiController
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _diamondText;
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
            //var money = info.Value;
            //_diamondText.text = money < 1000 ? money.ToString() : money < 1000000 ? $"{(float)money / 1000}K" : $"{(float)money / 1000000}ÊÊ";
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

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
        private void Init()
        {

        }

        private string CreateTextLabel(int currentValue, int maxValue)
        {
            var digitCount = (int)Math.Log10(Math.Abs(maxValue)) + 1;
            var format = "{0:d" + digitCount + "}";
            return $"{string.Format(format, currentValue)}/{maxValue}";
        }
        #endregion
    }
}