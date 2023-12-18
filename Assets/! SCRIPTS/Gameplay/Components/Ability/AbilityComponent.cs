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
                var ability = new Ability(type, _cooldowns.Find(e => e.Type == type).Duration);
                _abilities.Add(ability);
            }
        }

        private void CooldownAbilitys()
        {
            _abilities.ForEach(e => e.Cooldown(Time.deltaTime));
        }
        #endregion

        #region METHODS PUBLIC
        #endregion
    }
}
