using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Services.SignalSystem;
using Services.ScreenSystem;

namespace Gameplay
{
    public class RelaxerPopup : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _activateButton;

        [Space(10)]
        [SerializeField] private GameObject _relaxerContainer;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _progressFiller;
        #endregion

        #region FIELDS PRIVATE
        private RelaxerController _relaxer;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void ShowScreen(ShowScreenInfo info)
        {
            if (info.ScreenType != ScreenType.Relaxer) return;

            ShowScreen();
            SignalSystem<ScreenOpenedInfo>.Send(new(ScreenType.Relaxer));
        }

        [Subscribe]
        private void CloseScreen(CloseScreenInfo info)
        {
            if (info.ScreenType != ScreenType.Relaxer) return;
            CloseScreen();
        }

        [Subscribe]
        private void RelaxerChangeFocus(RelaxerChangeFocusInfo info)
        {
            _relaxer = info.Relaxer;
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
            _relaxerContainer.SetActive(false);
        }

        private void ShowRelaxerContainer()
        {
            _activateButton.SetActive(false);
            _relaxerContainer.SetActive(true);
        }
        #endregion

        #region METHODS PUBLIC
        public void ActivateButton()
        {
            ShowRelaxerContainer();

            _relaxer.TurnOn();
            _relaxer.OnTimerChange += TimerChangeHandler;
            _relaxer.OnProgressChange += ProgressChangeHandler;
            _relaxer.OnExploitationEnd += ExploitationEndHandler;

            SignalSystem<HidePlayerInfo>.Send(new());
            SignalSystem<InputControlInfo>.Send(new(false));
        }

        public void BreakButton()
        {
            CloseScreen();

            _relaxer.TurnOff();
            _relaxer.OnTimerChange -= TimerChangeHandler;
            _relaxer.OnProgressChange -= ProgressChangeHandler;
            _relaxer.OnExploitationEnd -= ExploitationEndHandler;

            SignalSystem<ShowPlayerInfo>.Send(new());
            SignalSystem<InputControlInfo>.Send(new(true));
        }
        #endregion
    }
}
