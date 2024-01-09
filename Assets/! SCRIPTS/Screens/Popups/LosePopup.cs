using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.ScreenSystem;
using Services.CurrencySystem;
using Services.SceneLoader;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class LosePopup : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Button _moneyButton;
        [SerializeField] private RectTransform _moneyContent;
        [SerializeField] private TextMeshProUGUI _moneyText;

        [Space(10)]
        [SerializeField] private Animator _dollAnimator;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ICurrencyService _currencyService;
        [Inject] private ISceneLoaderService _sceneLoaderService;

        private uint _money;
        #endregion

        #region HANDLERS
        private void MoneyButton()
        {
            _currencyService.PutCurrency(CurrencyType.Money, _money);

            Action callback = () => _signalService.Send<BatteryDischarge>(new());
            _sceneLoaderService.Load("2-WORLD", callback);
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();
            _moneyButton.onClick.AddListener(MoneyButton);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _moneyButton.onClick.RemoveListener(MoneyButton);
        }
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
            _money = (uint)payload;
            _moneyText.text = _money.ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_moneyContent);

            _dollAnimator.CrossFadeInFixedTime("Lose", 0.2f);
        }
        #endregion
    }
}
