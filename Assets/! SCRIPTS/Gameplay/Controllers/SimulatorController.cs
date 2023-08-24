using Cinemachine;
using EventHolder;
using System;
using System.Collections;
using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public enum EquipmentType
    {
        Simulator,
        Relaxer
    }

    public interface IEquipment
    {
        public EquipmentType Type { get; }
        public CurrencyType CurrencyType { get; }

        public event Action<float> OnTimerChange;
        public event Action<float> OnProgressChange;
        public event Action OnExploitationEnd;

        public void AddProgress(float progress);
        public void TurnOn();
        public void TurnOff();
    }

    public class SimulatorController : MonoBehaviour, IEquipment
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _id;
        [SerializeField] private EquipmentType _type;

        [Space(10)]
        [SerializeField, Range(0, 60)] private float _usageDuration;
        [SerializeField, Range(0, 100)] private float _progressForUsage;

        [Space(10)]
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private int _maxTokens;
        [SerializeField] private uint _costTokens;

        [Space(10)]
        [SerializeField] private CinemachineVirtualCamera _camera;
        #endregion

        #region FIELDS PRIVATE
        [Find] private RewardComponent _rewardComponent;

        private float _progress;
        private int _tokenCounter;
        #endregion

        #region PROPERTIES
        public EquipmentType Type => _type;
        public CurrencyType CurrencyType => _currencyType;
        #endregion

        #region EVENTS
        public event Action<float> OnTimerChange;
        public event Action<float> OnProgressChange;
        public event Action OnExploitationEnd;
        #endregion

        #region UNITY CALLBACKS
        #endregion

        #region METHODS PRIVATE
        private float NextProgressPoint(int points, int pointCounter)
        {
            return (100f / points) * pointCounter + 1;
        }
        #endregion

        #region METHODS PUBLIC
        public void TurnOn()
        {
            _progress = 0;
            _tokenCounter = 0;

            StartCoroutine(Exploitation(_usageDuration));

            _camera.Priority = 10;
        }

        public void TurnOff()
        {
            _camera.Priority = 0;
            StopAllCoroutines();
        }

        public void AddProgress(float value)
        {
            _progress += value;
            _progress = Mathf.Clamp(_progress, 0, 100f);
            OnProgressChange?.Invoke(_progress / 100f);

            if(_progress >= NextProgressPoint(_maxTokens, _tokenCounter))
            {
                _tokenCounter++;
                _rewardComponent.GiveOutReward(_currencyType, _costTokens, 1);
            }

            if(_progress == 100f)
            {
                OnExploitationEnd?.Invoke();
            }
        }
        #endregion

        #region COROUTINES
        private IEnumerator Exploitation(float duration)
        {
            var timer = duration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                OnTimerChange?.Invoke(timer);

                var delta = Time.deltaTime / duration;
                AddProgress(_progressForUsage * delta);

                yield return null;
            }

            OnExploitationEnd?.Invoke();
        }
        #endregion
    }
}
