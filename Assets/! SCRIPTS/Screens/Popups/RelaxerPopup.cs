using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.InputSystem;
using Services.SignalSystem.Signals;
using Services.ScreenSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class RelaxerPopup : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _activateButton;

        [Space(10)]
        [SerializeField] private GameObject _relaxerContainer;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Slider _progressFiller;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private IInputService _inputService;

        private RelaxerController _relaxer;
        #endregion

        #region HANDLERS
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

        #region METHODS PRIVATE
        private void SetRelaxer(object payload)
        {
            if (payload is RelaxerController relaxer)
            {
                _relaxer = relaxer;
            }
        }

        private void ShowActivateButton()
        {
            _activateButton.SetActive(true);
            _relaxerContainer.SetActive(false);
        }

        private void ShowRelaxerContainer()
        {
            _activateButton.SetActive(false);
            _relaxerContainer.SetActive(true);
        }
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
            SetRelaxer(payload);
            ShowActivateButton();
        }

        public void ActivateButton()
        {
            ShowRelaxerContainer();

            _relaxer.TurnOn();
            _relaxer.OnTimerChange += TimerChangeHandler;
            _relaxer.OnProgressChange += ProgressChangeHandler;
            _relaxer.OnExploitationEnd += ExploitationEndHandler;

            _signalService.Send<HidePlayer>(new());
            _inputService.EnableInputs = InputType.None;
        }

        public void BreakButton()
        {
            CloseScreen();

            _relaxer.TurnOff();
            _relaxer.OnTimerChange -= TimerChangeHandler;
            _relaxer.OnProgressChange -= ProgressChangeHandler;
            _relaxer.OnExploitationEnd -= ExploitationEndHandler;

            _signalService.Send<ShowPlayer>(new());
            _inputService.EnableInputs = InputType.Joystick;
        }
        #endregion
    }
}
