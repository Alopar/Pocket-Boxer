using UnityEngine;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.StateMachine;
using static Gameplay.CameraController;
using UpdateType = Utility.StateMachine.UpdateType;

namespace Gameplay
{
    public class FollowCameraState : AbstractCameraState
    {
        #region FIELDS PRIVATE
        private Vector3 _currentOffset = Vector3.zero;
        #endregion

        #region CONSTRUCTORS
        public FollowCameraState(CameraController entity, StateMachine stateMachine) : base(entity, stateMachine) {}
        #endregion

        #region HANDLERS
        [Subscribe(false)]
        private void InputDirection(InputDirection signal)
        {
            _currentOffset = new Vector3(_inputCameraOffset * signal.Direction.x, _inputCameraOffset * signal.Direction.y, _endOffset);
        }

        [Subscribe(false)]
        private void CameraLookAt(CameraLookAt signal)
        {
            _playerCamera.Priority = 0;
            _observingCamera.Priority = 10;
            _observingCamera.Follow = signal.LookAtPoint.transform;
        }

        [Subscribe(false)]
        private void CameraLookPlayer(CameraLookPlayer signal)
        {
            _playerCamera.Priority = 10;
            _observingCamera.Priority = 0;
        }

        [Subscribe(false)]
        private void CinemaStart(CinemaStart signal)
        {
            _stateMachine.ChangeState<CinemaCameraState>();
        }

        private void Update(UpdateType type)
        {
            switch (type)
            {
                case UpdateType.Update:
                    SetCameraOffset();
                    break;
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void SetCameraOffset()
        {
            _cameraOffset.m_Offset = _cameraOffset.m_Offset.sqrMagnitude <= 0.001f ? Vector3.zero : _cameraOffset.m_Offset;
            if (_currentOffset == Vector3.zero && _cameraOffset.m_Offset == Vector3.zero) return;

            _cameraOffset.m_Offset = Vector3.Lerp(_cameraOffset.m_Offset, _currentOffset, Time.deltaTime * 2f);
            _currentOffset = Vector3.zero;
        }
        #endregion

        #region METHODS PUBLIC
        public override void Enter()
        {
            _playerCamera.Priority = 10;
            _observingCamera.Priority = 0;

            _transmitter.CommonUpdate += Update;
            _signalService.Subscribe(this);
        }

        public override void Exit()
        {
            _transmitter.CommonUpdate -= Update;
            _signalService.Unsubscribe(this);
        }
        #endregion
    }
}
