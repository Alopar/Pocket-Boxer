using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.InputSystem;
using Services.ScreenSystem;
using Services.CurrencySystem;
using Services.TutorialSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class SimulatorPopup : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _activateButton;

        [Space(10)]
        [SerializeField] private GameObject _simulatorContainer;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Slider _progressFiller;
        [SerializeField] private Image _progressFillerImage;

        [Space(10)]
        [SerializeField] private GameObject _strengthIcon;
        [SerializeField] private GameObject _dexterityIcon;
        [SerializeField] private GameObject _enduranceIcon;

        [Space(10)]
        [SerializeField] private Color _strengthColor;
        [SerializeField] private Color _dexterityColor;
        [SerializeField] private Color _enduranceColor;

        [Space(10)]
        [SerializeField] private GameObject _tapContainer;
        [SerializeField] private GameObject _swipeContainer;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private IInputService _inputService;
        [Inject] private ITutorialService _tutorialService;

        private SimulatorController _simulator;
        #endregion

        #region HANDLERS
        private void InputSwipe(SwipeData data)
        {
            OverclockSimulator();
        }

        private void TimerChangeHandler(float value)
        {
            var min = (int)(value / 60);
            var sec = (int)(value % 60);
            _timerText.text = $"{min}:{sec:00}";
        }

        private void ProgressChangeHandler(float value)
        {
            _progressFiller.value = value;
        }

        private void ExploitationEndHandler()
        {
            BreakButton();
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();
            _inputService.OnSwipe += InputSwipe;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputService.OnSwipe -= InputSwipe;
        }
        #endregion

        #region METHODS PRIVATE
        private void SetSimulator(object payload)
        {
            if (payload is SimulatorController simulator)
            {
                _simulator = simulator;
            }
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

            SetFillerColor();
            ShowInputTools();
            ShowCurrencyIcon();
        }

        private void SetFillerColor()
        {
            switch (_simulator.CurrencyType)
            {
                case CurrencyType.StrengthPoints:
                    _progressFillerImage.color = _strengthColor;
                    break;
                case CurrencyType.DexterityPoints:
                    _progressFillerImage.color = _dexterityColor;
                    break;
                case CurrencyType.EndurancePoints:
                    _progressFillerImage.color = _enduranceColor;
                    break;
            }
        }

        private void ShowInputTools()
        {
            _tapContainer.SetActive(false);
            _swipeContainer.SetActive(false);
            switch (_simulator.SimulatorInputType)
            {
                case SimulatorInputType.Tap:
                    _tapContainer.SetActive(true);
                    break;
                case SimulatorInputType.Swipe:
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

        private void OverclockSimulator()
        {
            _simulator.AddProgress(5f);
            _simulator.ActivateNitro();
        }
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
            SetSimulator(payload);
            ShowActivateButton();
        }

        public void ActivateButton()
        {
            ShowSimulatorContainer();

            _simulator.TurnOn();
            _simulator.OnTimerChange += TimerChangeHandler;
            _simulator.OnProgressChange += ProgressChangeHandler;
            _simulator.OnExploitationEnd += ExploitationEndHandler;

            _inputService.EnableInputs = InputType.Swipe | InputType.Tap;
            _signalService.Send<HidePlayer>(new());
            _tutorialService.TriggerEvent(GameplayEvent.PushTrainButton);
        }

        public void BreakButton()
        {
            CloseScreen();

            _simulator.TurnOff();
            _simulator.OnTimerChange -= TimerChangeHandler;
            _simulator.OnProgressChange -= ProgressChangeHandler;
            _simulator.OnExploitationEnd -= ExploitationEndHandler;

            _inputService.EnableInputs = InputType.Joystick;
            _signalService.Send<ShowPlayer>(new());
        }

        public void Tap()
        {
            OverclockSimulator();
        }
        #endregion
    }
}
