using Services.InputSystem;
using Services.SignalSystem;
using Utility.StateMachine;
using Cinemachine;


namespace Gameplay
{
    public partial class CameraController
    {
        public abstract class AbstractCameraState : AbstractState
        {
            #region FIELDS PRIVATE
            protected readonly IInputService _inputService;
            protected readonly ISignalService _signalService;

            protected readonly MonoBehaviorTransmitter _transmitter;
            protected readonly CinemachineVirtualCamera _playerCamera;
            protected readonly CinemachineVirtualCamera _observingCamera;

            protected readonly float _startOffset;
            protected readonly float _endOffset;
            protected readonly float _observingTime;
            protected readonly CinemachineCameraOffset _cameraOffset;
            
            protected readonly float _inputCameraOffset;
            #endregion

            #region CONSTRUCTORS
            protected AbstractCameraState(CameraController entity, StateMachine stateMachine) : base(stateMachine)
            {
                _inputService = entity._inputService;
                _signalService = entity._signalsService;

                _transmitter = entity._transmitter;
                _playerCamera = entity._playerCamera;
                _observingCamera = entity._observingCamera;

                _startOffset = entity._startOffset;
                _endOffset = entity._endOffset;
                _observingTime = entity._observingTime;
                _cameraOffset = entity._cameraOffset;

                _inputCameraOffset = entity._inputCameraOffset;
            }
            #endregion
        }
    }
}
