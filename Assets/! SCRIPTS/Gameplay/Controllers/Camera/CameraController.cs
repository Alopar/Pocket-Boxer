using UnityEngine;
using Cinemachine;
using Services.InputSystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.StateMachine;
using Utility.DependencyInjection;
using DG.Tweening;

namespace Gameplay
{
    public partial class CameraController : MonoBehaviour, IDependant
    {
        #region FIELDS INSPECTOR
        [Header("INPUT SETTINGS:")]
        [SerializeField, Range(0, 10)] private float _inputCameraOffset = 1f;

        [Header("OBSERVING SETTINGS:")]
        [SerializeField, Range(0, 5)] private float _observingTime = 0.5f;
        [SerializeField] private float _startOffset = -10f;
        [SerializeField] private float _endOffset = 0f;

        [Header("LINKS:")]
        [SerializeField] private CinemachineVirtualCamera _playerCamera;
        [SerializeField] private CinemachineVirtualCamera _observingCamera;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalsService;
        [Inject] private IInputService _inputService;

        private StateMachine _stateMachine;
        private MonoBehaviorTransmitter _transmitter;

        private PlayerController _player;
        private CinemachineCameraOffset _cameraOffset;

        private Vector3 _currentFollowOffset;
        #endregion

        #region HANDLERS
        [Subscribe(false)]
        private void PlayerSpawn(PlayerSpawn info)
        {
            SetPlayer(info.PlayerController);
            _stateMachine.ChangeState<FollowCameraState>();
        }

        [Subscribe(false)]
        private void CameraChangeFOV(CameraChangeFOV info)
        {
            var currentFOV = _playerCamera.m_Lens.FieldOfView;
            DOVirtual.Float(currentFOV, info.FOV, _observingTime, (v) => { SetCameraFOV(v); }).SetEase(Ease.OutCubic);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        public void OnEnable()
        {
            _signalsService?.Subscribe(this);
        }

        private void OnDisable()
        {
            _signalsService?.Unsubscribe(this);
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _currentFollowOffset = _playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            _cameraOffset = _playerCamera.GetComponent<CinemachineCameraOffset>();
            _transmitter = gameObject.AddComponent<MonoBehaviorTransmitter>();

            InitializeStateMachine();
            SetCameraFOV(50f);
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new();
            _stateMachine.Initialization(new() {
                new CinemaCameraState(this, _stateMachine),
                new ObserverCameraState(this, _stateMachine),
                new FollowCameraState(this, _stateMachine),
            });
        }

        private void SetCameraFOV(float value)
        {
            var lens = _playerCamera.m_Lens;
            lens.FieldOfView = value;
            _playerCamera.m_Lens = lens;
        }

        private void SetPlayer(PlayerController player)
        {
            _player = player;
            if (_player == null)
            {
                Debug.LogError("player not found!");
                return;
            }

            _playerCamera.Follow = _player.transform;
        }
        #endregion
    }
}
