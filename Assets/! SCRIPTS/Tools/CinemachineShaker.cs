using UnityEngine;
using Cinemachine;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;
using NaughtyAttributes;

namespace Gameplay
{
    public class CinemachineShaker : MonoBehaviour, IDependant
    {
        #region FIELDS INSPECTOR
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField, Range(0, 10)] private float _shakeDuration;
        [SerializeField, Range(0, 10)] private float _shakeAmplitude;
        [SerializeField, Range(0, 10)] private float _shakeFrequency;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;

        private CinemachineBasicMultiChannelPerlin _channel;
        #endregion

        #region HANDLERS
        [Subscribe(false)]
        private void CameraShake(CameraShake signal)
        {
            StartShake(_shakeAmplitude, _shakeFrequency, _shakeDuration);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            _signalService.Subscribe(this);
        }

        private void OnDisable()
        {
            _signalService.Unsubscribe(this);
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _channel = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void StartShake(float amplitude, float frequency, float duration)
        {
            _channel.m_AmplitudeGain = amplitude;
            _channel.m_FrequencyGain = frequency;
            Invoke(nameof(StopShake), duration);
        }

        private void StopShake()
        {
            _channel.m_AmplitudeGain = 0f;
            _channel.m_FrequencyGain = 0f;
        }
        #endregion

        #region METHODS PUBLIC
        [Button("Shake")]
        public void Shake()
        {
            StartShake(_shakeAmplitude, _shakeFrequency, _shakeDuration);
        }
        #endregion
    }
}
