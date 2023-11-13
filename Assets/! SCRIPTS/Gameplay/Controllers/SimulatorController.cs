using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using Services.CurrencySystem;
using Utility.DependencyInjection;

namespace Gameplay
{

    [SelectionBase]
    public class SimulatorController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _id;

        [Space(10)]
        [SerializeField] private SimulatorInputType _simulatorInputType;
        [SerializeField, Range(0, 60)] private float _usageDuration;
        [SerializeField, Range(0, 100)] private float _progressForUsage;
        [SerializeField, Range(0, 100)] private uint _energyCost;

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
        private BatteryComponent _userBattery;
        #endregion

        #region PROPERTIES
        public SimulatorInputType SimulatorInputType => _simulatorInputType;
        public CurrencyType CurrencyType => _currencyType;
        public uint EnergyCost => _energyCost;
        #endregion

        #region EVENTS
        public event Action<float> OnTimerChange;
        public event Action<float> OnProgressChange;
        public event Action OnExploitationEnd;
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
            _userBattery?.TryGetEnergy(_energyCost);
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

        public void SetUserBattety(BatteryComponent battery)
        {
            _userBattery = battery;
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
