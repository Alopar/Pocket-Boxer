using EventHolder;
using Services.ScreenSystem;

namespace Gameplay
{
    public class SimulatorUiController : AbstractScreenController
    {
        #region FIELDS PRIVATE
        private SimulatorController _currentSimulator;
        #endregion

        #region HANDLERS
        [EventHolder]
        private void StartTrain(StartTrainInfo info)
        {
            _currentSimulator = info.Simulator;
            ShowScreen();
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
        #endregion
    }
}
