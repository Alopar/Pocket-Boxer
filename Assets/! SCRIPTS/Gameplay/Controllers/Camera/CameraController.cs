using UnityEngine;
using Cinemachine;
using EntityState;
using Services.SignalSystem;
using Utility.DependencyInjection;
using DG.Tweening;

namespace Gameplay
{

    public partial class CameraController : EntityMonoBehavior, IActivatable
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
        [Inject] private ISubscribeService _signals;

        private PlayerController _player;
        private CinemachineVirtualCamera _currentCamera;
        private CinemachineCameraOffset _cameraOffset;
        private MonoBehaviorTransmitter _transmitter;

        private Vector3 _currentFollowOffset;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void PlayerSpawn(PlayerSpawnInfo info)
        {
            SetPlayer(info.PlayerController);
            ChangeState(new FollowCameraState());
        }

        [Subscribe]
        private void CameraChangeFOV(CameraChangeFOVInfo info)
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
            _signals?.Subscribe(this);
        }

        private void OnDisable()
        {
            _signals?.Unsubscribe(this);
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _currentFollowOffset = _playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            _cameraOffset = _playerCamera.GetComponent<CinemachineCameraOffset>();
            _transmitter = gameObject.AddComponent<MonoBehaviorTransmitter>();

            SetCameraFOV(50f);
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
