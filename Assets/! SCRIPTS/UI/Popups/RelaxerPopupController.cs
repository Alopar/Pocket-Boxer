using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventHolder;
using Services.ScreenSystem;

namespace Gameplay
{
    public class RelaxerPopupController : AbstractScreenController
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
        [EventHolder]
        private void ShowScreen(ShowScreenInfo info)
        {
            if (info.ScreenType != ScreenType.Relaxer) return;

            ShowScreen();
            EventHolder<ScreenOpenedInfo>.NotifyListeners(new(ScreenType.Relaxer));
        }

        [EventHolder]
        private void CloseScreen(CloseScreenInfo info)
        {
            if (info.ScreenType != ScreenType.Relaxer) return;
            CloseScreen();
        }

        [EventHolder]
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

            EventHolder<HidePlayerInfo>.NotifyListeners(new());
            EventHolder<InputControlInfo>.NotifyListeners(new(false));
        }

        public void BreakButton()
        {
            CloseScreen();

            _relaxer.TurnOff();
            _relaxer.OnTimerChange -= TimerChangeHandler;
            _relaxer.OnProgressChange -= ProgressChangeHandler;
            _relaxer.OnExploitationEnd -= ExploitationEndHandler;

            EventHolder<ShowPlayerInfo>.NotifyListeners(new());
            EventHolder<InputControlInfo>.NotifyListeners(new(true));
        }
        #endregion
    }
}
