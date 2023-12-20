using System;
using UnityEngine;
using Screens.Layers.Arena;

namespace Gameplay
{
    public class Ability
    {
        #region FIELDS PRIVATE
        private readonly AbilityType _type;
        private readonly float _cooldownDuration;

        private AbilityState _state;
        private float _cooldownTimer;
        #endregion

        #region PROPERTIES
        public AbilityType Type => _type;
        public AbilityState State => _state;
        public float CooldownDuration => _cooldownDuration;
        public float CooldownTimer => _cooldownTimer;
        #endregion

        #region EVENTS
        public event Action<AbilityType, TargetZone> OnActivated;
        public event Action<AbilityType, AbilityState> OnStateChanged;
        public event Action<AbilityType, float, float> OnCooldownChanged;
        #endregion

        #region CONSTRUCTORS
        public Ability(AbilityType type, float cooldownDuration)
        {
            _type = type;
            _cooldownDuration = cooldownDuration;
        }
        #endregion

        #region METHODS PUBLIC
        public void TurnOn()
        {
            _state = AbilityState.Available;
            OnStateChanged?.Invoke(_type, _state);
        }

        public void TurnOff()
        {
            _state = AbilityState.Disabled;
            OnStateChanged?.Invoke(_type, _state);
        }

        public bool TryActivate(TargetZone zone)
        {
            if (_state != AbilityState.Available) return false;

            _state = AbilityState.Cooldown;
            OnStateChanged?.Invoke(_type, _state);
            _cooldownTimer = _cooldownDuration;

            OnActivated?.Invoke(_type, zone);

            return true;
        }

        public void Cooldown(float time)
        {
            if (_state != AbilityState.Cooldown) return;

            _cooldownTimer -= time;
            OnCooldownChanged?.Invoke(_type, _cooldownDuration, _cooldownTimer);

            if(_cooldownTimer <= 0)
            {
                _state = AbilityState.Available;
                OnStateChanged?.Invoke(_type, _state);
            }
        }
        #endregion
    }
}
