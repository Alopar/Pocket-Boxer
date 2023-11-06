using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.ScreenSystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    public class SimulatorPopup : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _activateButton;

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
        private SimulatorController _simulator;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void ShowScreen(ShowScreen info)
        {
            if (info.ScreenType != ScreenType.Simulator) return;

            ShowScreen();
            _signalService.Send<ScreenOpened>(new(ScreenType.Simulator));
        }

        [Subscribe]
        private void CloseScreen(CloseScreen info)
        {
            if (info.ScreenType != ScreenType.Simulator) return;
            CloseScreen();
        }

        [Subscribe]
        private void SimulatorChangeFocus(SimulatorChangeFocus info)
        {
            _simulator = info.Simulator;
        }

        [Subscribe]
        private void InputSwipe(InputSwipe signal)
        {
            _simulator.AddProgress(5f);
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
        #endregion

        #region METHODS PRIVATE
        protected override void ShowScreen()
        {
            base.ShowScreen();
            ShowActivateButton();
        }

        private void ShowActivateButton()
        {
            _activateButton.SetActive(true);
            _simulatorContainer.SetActive(false);
        }

        private void ShowSimulatorContainer()
        {
            _activateButton.SetActive(false);
            _simulatorContainer.SetActive(true);

            ShowInputTools();
            ShowCurrencyIcon();
        }

        private void ShowInputTools()
        {
            _tapContainer.SetActive(false);
            _swipeContainer.SetActive(false);
            switch (_simulator.InputType)
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
            switch (_simulator.CurrencyType)
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
            ShowSimulatorContainer();

            _simulator.TurnOn();
            _simulator.OnTimerChange += TimerChangeHandler;
            _simulator.OnProgressChange += ProgressChangeHandler;
            _simulator.OnExploitationEnd += ExploitationEndHandler;

            _signalService.Send<HidePlayer>(new());
            _signalService.Send<InputEnable>(new(false));
        }

        public void BreakButton()
        {
            CloseScreen();

            _simulator.TurnOff();
            _simulator.OnTimerChange -= TimerChangeHandler;
            _simulator.OnProgressChange -= ProgressChangeHandler;
            _simulator.OnExploitationEnd -= ExploitationEndHandler;

            _signalService.Send<ShowPlayer>(new());
            _signalService.Send<InputEnable>(new(true));
        }

        public void Tap()
        {
            _simulator.AddProgress(5f);
        }
        #endregion
    }
}
