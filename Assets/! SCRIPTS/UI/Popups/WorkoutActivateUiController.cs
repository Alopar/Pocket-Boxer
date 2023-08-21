using EventHolder;
using Services.ScreenSystem;

namespace Gameplay
{
    public class WorkoutActivateUiController : AbstractScreenController
    {
        #region FIELDS PRIVATE
        private SimulatorController _currentSimulator;
        #endregion

        #region HANDLERS
        [EventHolder]
        private void ShowSimulatorButton(ShowSimulatorButtonInfo info)
        {
            _currentSimulator = info.Simulator;

            ShowScreen();
        }

        [EventHolder]
        private void CloseSimulatorButton(CloseSimulatorButtonInfo info)
        {
            CloseScreen();
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

        #region METHODS PUBLIC
        public override void CloseScreen()
        {
            base.CloseScreen();
        }

        public void TrainButton()
        {
            EventHolder<StartTrainInfo>.NotifyListeners(new(_currentSimulator));
            CloseScreen();
        }
        #endregion
    }
}
