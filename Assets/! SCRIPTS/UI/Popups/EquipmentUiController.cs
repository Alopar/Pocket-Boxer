using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventHolder;
using Services.ScreenSystem;

namespace Gameplay
{
    public class EquipmentUiController : AbstractScreenController
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _activateButton;
        [SerializeField] private TextMeshProUGUI _activateButtonText;

        [Space(10)]
        [SerializeField] private GameObject _simulatorContainer;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _progressFiller;

        [Space(10)]
        [SerializeField] private GameObject _strengthIcon;
        [SerializeField] private GameObject _dexterityIcon;
        [SerializeField] private GameObject _enduranceIcon;

        [Space(10)]
        [SerializeField] private GameObject _tapContainer;
        [SerializeField] private GameObject _swipeContainer;
        #endregion

        #region FIELDS PRIVATE
        private IEquipment _equipment;
        #endregion

        #region HANDLERS
        [EventHolder]
        private void ShowEquipmentScreen(ShowEquipmentScreenInfo info)
        {
            _equipment = info.Equipment;
            switch (_equipment.Type)
            {
                case EquipmentType.Simulator:
                    _activateButtonText.text = "TRAIN";
                    break;
                case EquipmentType.Relaxer:
                    _activateButtonText.text = "SLEEP";
                    break;
            }

            ShowScreen();
        }

        [EventHolder]
        private void CloseEquipmentSceen(CloseEquipmentSceenInfo info)
        {
            CloseScreen();
        }

        [EventHolder]
        private void InputSwipe(InputSwipeInfo info)
        {
            _equipment.AddProgress(5f);
        }

        private void TimerChangeHandler(float value)
        {
            var min = (int)(value / 60);
            var sec = (int)(value % 60);
            _timerText.text = $"{min}:{sec:00}";
        }

        private void ProgressChangeHandler(float value)
        {
            _progressFiller.fillAmount = value;
        }

        private void ExploitationEndHandler()
        {
            BreakButton();
        }
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            HideScreen();
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
        protected override void ShowScreen()
        {
            base.ShowScreen();
            _activateButton.SetActive(true);
            _simulatorContainer.SetActive(false);
        }

        private void ShowSimulatorContainer()
        {
            _simulatorContainer.SetActive(true);

            ShowInputTools();
            ShowCurrencyIcon();
        }

        private void ShowInputTools()
        {
            _tapContainer.SetActive(false);
            _swipeContainer.SetActive(false);
            switch (_equipment.InputType)
            {
                case InputType.Tap:
                    _tapContainer.SetActive(true);
                    break;
                case InputType.Swipe:
                    _swipeContainer.SetActive(true);
                    break;
            }
        }

        private void ShowCurrencyIcon()
        {
            _strengthIcon.SetActive(false);
            _dexterityIcon.SetActive(false);
            _enduranceIcon.SetActive(false);
            switch (_equipment.CurrencyType)
            {
                case CurrencyType.StrengthPoints:
                    _strengthIcon.SetActive(true);
                    break;
                case CurrencyType.DexterityPoints:
                    _dexterityIcon.SetActive(true);
                    break;
                case CurrencyType.EndurancePoints:
                    _enduranceIcon.SetActive(true);
                    break;
            }
        }
        #endregion

        #region METHODS PUBLIC
        public void ActivateButton()
        {
            _activateButton.SetActive(false);

            switch (_equipment.Type)
            {
                case EquipmentType.Simulator:
                    ShowSimulatorContainer();
                    break;
                case EquipmentType.Relaxer:
                    break;
            }

            _equipment.TurnOn();
            _equipment.OnTimerChange += TimerChangeHandler;
            _equipment.OnProgressChange += ProgressChangeHandler;
            _equipment.OnExploitationEnd += ExploitationEndHandler;

            EventHolder<HidePlayerInfo>.NotifyListeners(new());
            EventHolder<InputControlInfo>.NotifyListeners(new(false));
        }

        public void BreakButton()
        {
            CloseScreen();

            _equipment.TurnOff();
            _equipment.OnTimerChange -= TimerChangeHandler;
            _equipment.OnProgressChange -= ProgressChangeHandler;
            _equipment.OnExploitationEnd -= ExploitationEndHandler;

            EventHolder<ShowPlayerInfo>.NotifyListeners(new());
            EventHolder<InputControlInfo>.NotifyListeners(new(true));
        }

        public void Tap()
        {
            _equipment.AddProgress(5f);
        }
        #endregion
    }
}
