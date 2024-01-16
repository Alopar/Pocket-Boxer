using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Services.CurrencySystem;
using Utility.DependencyInjection;
using DG.Tweening;
using QuickOutline;

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

        [Space(10)]
        [SerializeField] private Transform _dollPoint;
        [SerializeField] private CharacterAnimation _dollAnimation;

        [Space(10)]
        [SerializeField] private GameObject _icon;

        [Space(10)]
        [SerializeField] private AbstaractSimulatorAnimation _simulatorAnimation;

        [Space(10)]
        [SerializeField] private List<GameObject> _IdleStateEquipments;
        [SerializeField] private List<GameObject> _WorkStateEquipments;

        [Space(10)]
        [SerializeField] private Outline _outline;
        #endregion

        #region FIELDS PRIVATE
        [Find] private RewardComponent _rewardComponent;

        private float _progress;
        private int _tokenCounter;
        private Manikin _manikin;
        private BatteryComponent _userBattery;

        private Tween _nitroTimer;
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

        #region HANDLERS
        private void ManikinHit()
        {
            _simulatorAnimation?.PlayShot();
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            FocusOff();
        }

        private float NextProgressPoint(int points, int pointCounter)
        {
            return (100f / points) * pointCounter + 1;
        }
        #endregion

        #region METHODS PUBLIC
        public void FocusOn()
        {
            _outline.enabled = true;
        }

        public void FocusOff()
        {
            _outline.enabled = false;
        }

        public void TurnOn()
        {
            _progress = 0;
            _tokenCounter = 0;
            _camera.Priority = 10;

            FocusOff();
            _simulatorAnimation?.TurnOn();

            _manikin?.Activate(_dollAnimation);
            _manikin.SetAnimationSpeed(0.5f);

            _userBattery?.TryGetEnergy(_energyCost);
            StartCoroutine(Exploitation(_usageDuration));

            _IdleStateEquipments.ForEach(e => e.SetActive(false));
            _WorkStateEquipments.ForEach(e => e.SetActive(true));

            _icon.SetActive(false);
        }

        public void TurnOff()
        {
            _camera.Priority = 0;
            _nitroTimer?.Complete();
            _simulatorAnimation?.TurnOff();

            RemoveDoll();
            StopAllCoroutines();

            _IdleStateEquipments.ForEach(e => e.SetActive(true));
            _WorkStateEquipments.ForEach(e => e.SetActive(false));

            _icon.SetActive(true);
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

        public void ActivateNitro()
        {
            _manikin?.SetAnimationSpeed(1f);

            _nitroTimer?.Complete();
            _nitroTimer = DOVirtual.Float(1f, 0.5f, 2f, (value) => { _manikin?.SetAnimationSpeed(value); });
        }

        public void SetDoll(GameObject doll)
        {
            _manikin = new Manikin(doll, _dollPoint);
            _manikin.OnHit += ManikinHit;
        }

        public void RemoveDoll()
        {
            if(_manikin != null)
            {
                _manikin.Dispose();
                _manikin.OnHit -= ManikinHit;
                _manikin = null;
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
