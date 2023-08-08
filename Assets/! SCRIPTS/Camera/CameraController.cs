using UnityEngine;
using Cinemachine;
using EntityState;
using EventHolder;
using DG.Tweening;

namespace Gameplay
{
    public partial class CameraController : EntityMonoBehavior
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
        private PlayerController _player;
        private CinemachineVirtualCamera _currentCamera;
        private CinemachineCameraOffset _cameraOffset;
        private MonoBehaviorTransmitter _transmitter;

        private Vector3 _currentFollowOffset;
        #endregion

        #region HANDLERS
        [EventHolder]
        private void PlayerSpawn(PlayerSpawnInfo info)
        {
            SetPlayer(info.PlayerController);
            ChangeState(new FollowCameraState());
        }

        [EventHolder]
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
