using System;
using System.Collections.Generic;
using UnityEngine;
using Screens.Layers.Arena;

namespace Gameplay
{
    public class AbilityComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<AbilityCooldownTime> _cooldowns;
        #endregion

        #region FIELDS PRIVATE
        private List<Ability> _abilities = new();
        #endregion

        #region PROPERTIES
        public List<Ability> Abilities => _abilities;
        #endregion

        #region EVENTS
        public event Action<AbilityType, TargetZone> OnAbility;
        #endregion

        #region HANDLERS
        public void AbilityActivated(AbilityType type, TargetZone zone)
        {
            OnAbility?.Invoke(type, zone);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            _abilities.ForEach(e => e.TurnOn());
        }

        private void LateUpdate()
        {
            CooldownAbilitys();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            foreach (AbilityType type in Enum.GetValues(typeof(AbilityType)))
            {
                if(type == AbilityType.None) continue;

                var ability = new Ability(type, _cooldowns.Find(e => e.Type == type).Duration);
                ability.OnActivated += AbilityActivated;
                _abilities.Add(ability);
            }
        }

        private void CooldownAbilitys()
        {
            _abilities.ForEach(e => e.Cooldown(Time.deltaTime));
        }
        #endregion
    }
}
