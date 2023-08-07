using UnityEngine;
using EventHolder;
using TMPro;
using DG.Tweening;

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
        private void h_MoneyChange(MoneyChangeInfo info)
        {
            var money = info.Value;
            _moneyText.text = money < 1000 ? money.ToString() : money < 1000000 ? $"{(float)money / 1000}K" : $"{(float)money / 1000000}ÊÊ";
        }

        private void h_DiamondChange(DiamondChangeInfo info)
        {
            var money = info.Value;
            _diamondText.text = money < 1000 ? money.ToString() : money < 1000000 ? $"{(float)money / 1000}K" : $"{(float)money / 1000000}ÊÊ";
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            EventHolder<MoneyChangeInfo>.AddListener(h_MoneyChange, true);
            EventHolder<DiamondChangeInfo>.AddListener(h_DiamondChange, true);
        }

        private void OnDisable()
        {
            EventHolder<MoneyChangeInfo>.RemoveListener(h_MoneyChange);
            EventHolder<DiamondChangeInfo>.RemoveListener(h_DiamondChange);
        }
        #endregion
    }
}