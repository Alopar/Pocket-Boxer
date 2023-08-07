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

        [Header("CITY SETTINGS:")]
        [SerializeField, Range(0, 5)] private float _zoomTime = 0.5f;
        [SerializeField, Range(0, 5)] private float _zoomRatio = 2f;
        [SerializeField] private Ease _zoomEase = Ease.Linear;

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
        private void h_PlayerSpawn(PlayerSpawnInfo info)
        {
            Init(info.PlayerController);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            EventHolder<PlayerSpawnInfo>.AddListener(h_PlayerSpawn, true);
        }

        private void OnDisable()
        {
            EventHolder<PlayerSpawnInfo>.RemoveListener(h_PlayerSpawn);
        }
        #endregion

        #region METHODS PUBLIC
        public void Init(PlayerController player)
        {
            _currentFollowOffset = _playerCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

            _player = player;
            if (_player == null)
            {
                Debug.LogError("player not found!");
                return;
            }

            _playerCamera.Follow = _player.transform;
            _cameraOffset = _playerCamera.GetComponent<CinemachineCameraOffset>();
            _transmitter = gameObject.AddComponent<MonoBehaviorTransmitter>();

            ChangeState(new ObserverCameraState());
        }
        #endregion
    }
}
