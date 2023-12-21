using System;
using UnityEngine;
using Tools;
using Screens.Layers.Arena;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.MonoPool;
using Utility.DependencyInjection;

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

        [Header("BALANCE SETTINGS:")]
        [SerializeField] private int _healthModifier;
        [SerializeField] private int _cooldownModifier;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Find] private AbilityComponent _abilityComponent;

        private BoxerState _state;
        private AbilityType _ability = AbilityType.None;
        private TargetZone _zone = TargetZone.None;

        private int _strength;
        private int _dexterity;
        private int _endurance;

        private int _maxHP;
        private int _currentHP;
        #endregion

        #region PROPERTIES
        public BoxerState State => _state;
        public AbilityType Ability => _ability;
        public TargetZone TargetZone => _zone;
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
            if (signal.ControleType == _controleType) return;
            if (_state == BoxerState.Death || _state == BoxerState.Victory) return;
            if (_ability == AbilityType.Block || _ability == AbilityType.Dodge) return;
            
            DealDamage(signal.Damage);
            ShowNumerics(signal.Damage);
            DropAbility();

            if (_currentHP > 0)
            {
                var animationName = "TopHit";
                switch (signal.TargetZone)
                {
                    case TargetZone.Top:
                        animationName = "HitHead";
                        break;
                    case TargetZone.Middle:
                        animationName = "HitBody";
                        break;
                    case TargetZone.Bottom:
                        animationName = "HitLeg";
                        break;
                }

                ChangeAnimation(animationName);
            }
            else
            {
                ChangeState(BoxerState.Death);
                ChangeAnimation("DeathBackward");
                _signalService.Send<Defeat>(new(_controleType));
            }
        }

        [Subscribe(false)]
        private void Defeat(Defeat signal)
        {
            if (signal.ControleType == _controleType) return;

            ChangeState(BoxerState.Victory);
            ChangeAnimation("Victory");
        }

        private void AnimationStance(byte index)
        {
            if (_state == BoxerState.Stance) return;
            ChangeState(BoxerState.Stance);
        }

        private void AnimationStrike(byte index)
        {
            var damage = CalculateDamage(_ability, _zone);
            _signalService.Send<Strike>(new(_ability, _zone, _controleType, damage));
            DropAbility();
        }

        private void AnimationEnd(byte index)
        {
            ChangeAnimation("FightingStance");
            DropAbility();
        }

        private void AbilityActivated(AbilityType type, TargetZone zone)
        {
            if (_state == BoxerState.Death || _state == BoxerState.Victory) return;

            _ability = type;
            _zone = zone;

            string animationName = null;
            switch (_ability)
            {
                case AbilityType.Block:
                    animationName = "Block";
                    break;
                case AbilityType.Dodge:
                    animationName = "Dodge";
                    break;
                case AbilityType.Headbutt:
                    animationName = "Headbutt";
                    break;
                case AbilityType.HandKick:
                    switch (_zone)
                    {
                        case TargetZone.Top:
                            animationName = "TopHandKick";
                            break;
                        case TargetZone.Middle:
                            animationName = "MiddleHandKick";
                            break;
                        case TargetZone.Bottom:
                            animationName = "BottomHandKick";
                            break;
                    }
                    break;
                case AbilityType.FootKick:
                    switch (_zone)
                    {
                        case TargetZone.Top:
                            animationName = "TopFootKick";
                            break;
                        case TargetZone.Middle:
                            animationName = "MiddleFootKick";
                            break;
                        case TargetZone.Bottom:
                            animationName = "BottomFootKick";
                            break;
                    }
                    break;
            }

            ChangeState(BoxerState.Action);
            ChangeAnimation(animationName);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            _signalService.Subscribe(this);

            _abilityComponent.OnAbility += AbilityActivated;
            _animationTransmitter.AnimationEvent01 += AnimationStance;
            _animationTransmitter.AnimationEvent04 += AnimationStrike;
            _animationTransmitter.AnimationEvent05 += AnimationEnd;
        }

        private void OnDisable()
        {
            _signalService.Unsubscribe(this);

            _abilityComponent.OnAbility -= AbilityActivated;
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
            ChangeAnimation("FightingStance");
            OnHealthChange?.Invoke(_controleType, _currentHP, _maxHP);
        }

        private void ChangeState(BoxerState state)
        {
            _state = state;
            OnStateChange?.Invoke(_state);
        }

        private void DealDamage(int value)
        {
            _currentHP -= value;
            OnHealthChange?.Invoke(_controleType, _currentHP, _maxHP);
        }

        private void ShowNumerics(int number)
        {
            var numeric = MonoPool.Instantiate(_floatupHpPrefab);
            numeric.transform.position = _floatupHpPoint.position;
            numeric.Init(number);
        }

        private void ChangeAnimation(string name)
        {
            _animator.CrossFadeInFixedTime(name, 0.2f);
        }

        private void DropAbility()
        {
            _ability = AbilityType.None;
        }

        private int CalculateDamage(AbilityType ability, TargetZone zone)
        {
            var damage = 0;
            switch (ability)
            {
                case AbilityType.Headbutt:
                    damage = _strength * 5;
                    break;
                case AbilityType.HandKick:
                    switch (zone)
                    {
                        case TargetZone.Top:
                            damage = _strength * 3;
                            break;
                        case TargetZone.Middle:
                            damage = _strength * 1;
                            break;
                        case TargetZone.Bottom:
                            damage = _strength * 2;
                            break;
                    }
                    break;
                case AbilityType.FootKick:
                    switch (zone)
                    {
                        case TargetZone.Top:
                            damage = _strength * 4;
                            break;
                        case TargetZone.Middle:
                            damage = _strength * 2;
                            break;
                        case TargetZone.Bottom:
                            damage = _strength * 3;
                            break;
                    }
                    break;
            }

            return damage;
        }
        #endregion

        #region METHODS PUBLIC
        public void SetStats(int strength, int dexterity, int endurance)
        {
            _strength = strength;
            _dexterity = dexterity;
            _endurance = endurance;

            _maxHP = _endurance * _healthModifier;
            _currentHP = _maxHP;

            var modifier = 1f + ((float)_dexterity / _cooldownModifier);
            _abilityComponent.SetCooldownModifier(modifier);
        }
        #endregion
    }
}
