using UnityEngine;
using Services.ScreenSystem;
using Services.SignalSystem.Signals;
using Gameplay;
using Services.SignalSystem;

namespace Screens.Layers.Arena
{
    public class AbilityLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private AbilityButton _headButton;
        [SerializeField] private AbilityButton _blockButton;
        [SerializeField] private AbilityButton _dodgeButton;

        [Space(10)]
        [SerializeField] private AbilityJoystick _handJoystic;
        [SerializeField] private AbilityJoystick _footJoystic;
        #endregion

        #region FIELDS PRIVATE
        private BoxerController _boxer;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void BoxerSpawn(BoxerSpawn signal)
        {
            _boxer = signal.BoxerController;
            SubsctibeAbilityEvents();
        }

        private void UseAbility(AbilityType type)
        {
            var ability = _boxer.AbilityComponent.Abilities.Find(e => e.Type == type);
            ability.TryActivate();
        }

        private void AbilityStateChanged(AbilityType type, AbilityState state)
        {
            var buttonState = AbilityButtonState.Locked;
            switch (state)
            {
                case AbilityState.Disabled:
                    buttonState = AbilityButtonState.Disable;
                    break;
                case AbilityState.Available:
                    buttonState = AbilityButtonState.Active;
                    break;
                case AbilityState.Cooldown:
                    buttonState = AbilityButtonState.Cooldown;
                    break;
            }

            switch (type)
            {
                case AbilityType.Block:
                    _blockButton.SetState(buttonState);
                    break;
                case AbilityType.Dodge:
                    _dodgeButton.SetState(buttonState);
                    break;
                case AbilityType.Headbutt:
                    _headButton.SetState(buttonState);
                    break;
                case AbilityType.HandKick:
                    _handJoystic.SetState(buttonState);
                    break;
                case AbilityType.FootKick:
                    _footJoystic.SetState(buttonState);
                    break;
            }
        }

        private void AbilityCooldownChanged(AbilityType type, float duration, float value)
        {
            var delta = 1f - (value / duration);
            switch (type)
            {
                case AbilityType.Block:
                    _blockButton.SetCooldown(delta);
                    break;
                case AbilityType.Dodge:
                    _dodgeButton.SetCooldown(delta);
                    break;
                case AbilityType.Headbutt:
                    _headButton.SetCooldown(delta);
                    break;
                case AbilityType.HandKick:
                    _handJoystic.SetCooldown(delta);
                    break;
                case AbilityType.FootKick:
                    _footJoystic.SetCooldown(delta);
                    break;
            }
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeAbilityTriggers();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnsubscribeAbilityTriggers();
            UnsubsctibeAbilityEvents();
        }
        #endregion

        #region METHODS PRIVATE
        private void SubscribeAbilityTriggers()
        {
            _blockButton.OnAbility += UseAbility;
            _headButton.OnAbility += UseAbility;
            _dodgeButton.OnAbility += UseAbility;

            _handJoystic.OnAbility += UseAbility;
            _footJoystic.OnAbility += UseAbility;
        }

        private void UnsubscribeAbilityTriggers()
        {
            _blockButton.OnAbility -= UseAbility;
            _headButton.OnAbility -= UseAbility;
            _dodgeButton.OnAbility -= UseAbility;

            _handJoystic.OnAbility -= UseAbility;
            _footJoystic.OnAbility -= UseAbility;
        }

        //private void ActivateButtonStates()
        //{
        //    _blockButton.SetState(AbilityButtonState.Active);
        //    _headButton.SetState(AbilityButtonState.Active);
        //    _dodgeButton.SetState(AbilityButtonState.Active);

        //    _handJoystic.SetState(AbilityButtonState.Active);
        //    _footJoystic.SetState(AbilityButtonState.Active);
        //}

        private void SubsctibeAbilityEvents()
        {
            foreach (var ability in _boxer.AbilityComponent.Abilities)
            {
                ability.OnStateChanged += AbilityStateChanged;
                ability.OnCooldownChanged += AbilityCooldownChanged;
            }
        }

        private void UnsubsctibeAbilityEvents()
        {
            foreach (var ability in _boxer.AbilityComponent.Abilities)
            {
                ability.OnStateChanged -= AbilityStateChanged;
                ability.OnCooldownChanged -= AbilityCooldownChanged;
            }
        }
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
            //ActivateButtonStates();
        }
        #endregion
    }
}
