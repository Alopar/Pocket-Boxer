using System;
using UnityEngine;
using Tools;
using Screens.Layers.Arena;
using Services.SignalSystem;
using Utility.DependencyInjection;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    [SelectionBase]
    public class BoxerController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private ControleType _controleType;

        [Header("BASE SETTINGS:")]
        [SerializeField] private Transform _view;
        [SerializeField] private Transform _informers;

        [Space(10)]
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationEventTransmitter _animationTransmitter;

        [Space(10)]
        [SerializeField] private GameObject _doll;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Find] private AbilityComponent _abilityComponent;

        private BoxerState _state;
        private AbilityType _currentAbility;
        #endregion

        #region PROPERTIES
        public ControleType ControleType => _controleType;
        public AbilityComponent AbilityComponent => _abilityComponent;
        #endregion

        #region EVENTS
        public event Action<BoxerState> OnStateChange;
        #endregion

        #region HANDLERS
        [Subscribe(false)]
        private void Strike(Strike signal)
        {
            if (signal.ControleType == _controleType) return;
            if (_currentAbility == AbilityType.Block || _currentAbility == AbilityType.Dodge) return;

            _animator.CrossFadeInFixedTime("TopHit", 0.2f);
        }

        private void AnimationStance(byte index)
        {
            if (_state == BoxerState.Stance) return;

            _state = BoxerState.Stance;
            OnStateChange?.Invoke(_state);
        }

        private void AnimationStrike(byte index)
        {
            _signalService.Send<Strike>(new(_currentAbility, _controleType));
        }

        private void AnimationEnd(byte index)
        {
            _animator.CrossFadeInFixedTime("FightingStance", 0.2f);
        }

        private void Ability(AbilityType type)
        {
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
        }
        #endregion
    }
}
