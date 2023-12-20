using System;
using UnityEngine;
using Tools;
using Screens.Layers.Arena;
using Services.SignalSystem;
using Utility.DependencyInjection;
using Services.SignalSystem.Signals;
using Utility.MonoPool;

namespace Gameplay
{
    [SelectionBase]
    public class BoxerController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _name;
        [SerializeField] private ControleType _controleType;

        [Header("BASE SETTINGS:")]
        [SerializeField] private Transform _view;
        [SerializeField] private Transform _informers;

        [Space(10)]
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationEventTransmitter _animationTransmitter;

        [Space(10)]
        [SerializeField] private GameObject _doll;

        [Space(10)]
        [SerializeField] private Transform _floatupHpPoint;
        [SerializeField] private FloatupNumeric _floatupHpPrefab;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Find] private AbilityComponent _abilityComponent;

        private BoxerState _state;
        private AbilityType _currentAbility;

        private int _strength;
        private int _dexterity;
        private int _endurance;

        private int _maxHP;
        private int _currentHP;
        #endregion

        #region PROPERTIES
        public ControleType ControleType => _controleType;
        public AbilityComponent AbilityComponent => _abilityComponent;
        public string Name => _name;
        #endregion

        #region EVENTS
        public event Action<BoxerState> OnStateChange;
        public event Action<ControleType, int, int> OnHealthChange;
        #endregion

        #region HANDLERS
        [Subscribe(false)]
        private void Strike(Strike signal)
        {
            if (_state == BoxerState.Death) return;
            if (signal.ControleType == _controleType) return;
            if (_currentAbility == AbilityType.Block || _currentAbility == AbilityType.Dodge) return;

            _currentHP -= signal.Damage;
            OnHealthChange?.Invoke(_controleType, _currentHP, _maxHP);

            var numeric = MonoPool.Instantiate(_floatupHpPrefab);
            numeric.transform.position = _floatupHpPoint.position;
            numeric.Init(signal.Damage);

            if (_currentHP <= 0)
            {
                _state = BoxerState.Death;
                OnStateChange?.Invoke(_state);

                _signalService.Send<Defeat>(new(_controleType));

                _animator.CrossFadeInFixedTime("DeathBackward", 0.2f);
            }
            else
            {
                _animator.CrossFadeInFixedTime("TopHit", 0.2f);
            }
        }

        [Subscribe(false)]
        private void Defeat(Defeat signal)
        {
            if (signal.ControleType == _controleType) return;

            _state = BoxerState.Victory;
            OnStateChange?.Invoke(_state);

            _animator.CrossFadeInFixedTime("Victory", 0.2f);
        }

        private void AnimationStance(byte index)
        {
            if (_state == BoxerState.Stance) return;

            _state = BoxerState.Stance;
            OnStateChange?.Invoke(_state);
        }

        private void AnimationStrike(byte index)
        {
            var damage = 0;
            damage = _strength * 2;

            _signalService.Send<Strike>(new(_currentAbility, _controleType, damage));
        }

        private void AnimationEnd(byte index)
        {
            _animator.CrossFadeInFixedTime("FightingStance", 0.2f);
        }

        private void Ability(AbilityType type)
        {
            if (_state == BoxerState.Death) return;
            if (_state == BoxerState.Victory) return;

            _currentAbility = type;
            switch (type)
            {
                case AbilityType.Block:
                    _animator.CrossFadeInFixedTime("Block", 0.2f);
                    break;
                case AbilityType.Dodge:
                    _animator.CrossFadeInFixedTime("Dodge", 0.2f);
                    break;
                case AbilityType.Headbutt:
                    _animator.CrossFadeInFixedTime("Headbutt", 0.2f);
                    break;
                case AbilityType.HandKick:
                    _animator.CrossFadeInFixedTime("TopHandKick", 0.2f);
                    break;
                case AbilityType.FootKick:
                    _animator.CrossFadeInFixedTime("TopFootKick", 0.2f);
                    break;
            }

            _state = BoxerState.Action;
            OnStateChange?.Invoke(_state);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            _signalService.Subscribe(this);

            _abilityComponent.OnAbility += Ability;
            _animationTransmitter.AnimationEvent01 += AnimationStance;
            _animationTransmitter.AnimationEvent04 += AnimationStrike;
            _animationTransmitter.AnimationEvent05 += AnimationEnd;
        }

        private void OnDisable()
        {
            _signalService.Unsubscribe(this);

            _abilityComponent.OnAbility -= Ability;
            _animationTransmitter.AnimationEvent01 -= AnimationStance;
            _animationTransmitter.AnimationEvent04 -= AnimationStrike;
            _animationTransmitter.AnimationEvent05 -= AnimationEnd;
        }

        private void Start()
        {
            Init();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _animator.CrossFadeInFixedTime("FightingStance", 0.2f);
            OnHealthChange?.Invoke(_controleType, _currentHP, _maxHP);
        }
        #endregion

        #region METHODS PUBLIC
        public void SetStats(int strength, int dexterity, int endurance)
        {
            _strength = strength;
            _dexterity = dexterity;
            _endurance = endurance;

            _maxHP = _endurance * 50;
            _currentHP = _maxHP;
        }
        #endregion
    }
}
