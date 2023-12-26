using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.ScreenSystem;
using Services.CurrencySystem;
using Utility.DependencyInjection;
using DG.Tweening;
using Services.SceneLoader;

namespace Gameplay
{
    public class WinPopup : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Button _moneyButton;
        [SerializeField] private RectTransform _moneyContent;
        [SerializeField] private TextMeshProUGUI _moneyText;

        [Space(10)]
        [SerializeField] private RectTransform _rays;
        [SerializeField, Range(0, 99)] private float _rotateTimeCycle = 10f;

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
            _sceneLoaderService.Load("2-WORLD");
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();
            _rays.DOLocalRotate(new Vector3(0, 0, 360), _rotateTimeCycle, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
            _moneyButton.onClick.AddListener(MoneyButton);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _rays.DOKill();
            _moneyButton.onClick.RemoveListener(MoneyButton);
        }
        #endregion

        #region METHODS PRIVATE
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
            _money = (uint)payload;
            _moneyText.text = _money.ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_moneyContent);

            _dollAnimator.CrossFadeInFixedTime("Win", 0.2f);
        }
        #endregion
    }
}
