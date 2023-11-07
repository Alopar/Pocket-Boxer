using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.StateMachine;
using static Gameplay.CameraController;

namespace Gameplay
{
    public class CinemaCameraState : AbstractCameraState
    {
        #region CONSTRUCTORS
        public CinemaCameraState(CameraController entity, StateMachine stateMachine) : base(entity, stateMachine) {}
        #endregion

        #region HANDLERS
        [Subscribe]
        private void CinemaFinish(CinemaFinish signal)
        {
            _stateMachine.ChangeState<FollowCameraState>();
        }
        #endregion

        #region METHODS PUBLIC
        public override void Enter()
        {
            _playerCamera.Priority = 0;
            _observingCamera.Priority = 0;

            _signalService.Subscribe(this);
        }

        public override void Exit()
        {
            _signalService.Unsubscribe(this);
        }
        #endregion
    }
}
