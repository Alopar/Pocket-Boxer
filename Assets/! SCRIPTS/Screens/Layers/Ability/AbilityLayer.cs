using UnityEngine;
using Services.ScreenSystem;

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

        #region HANDLERS
        private void UseAbility(AbilityType type)
        {
            Debug.Log(type.ToString());
            switch (type)
            {
                case AbilityType.Block:
                    break;
                case AbilityType.Dodge:
                    break;
                case AbilityType.Headbutt:
                    break;
                case AbilityType.HandKick:
                    break;
                case AbilityType.FootKick:
                    break;
            }
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            SubscribeAbilityTriggers();
        }

        protected override void OnDisable()
        {
            UnsubscribeAbilityTriggers();
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

        private void ActivateButtonStates()
        {
            _blockButton.SetState(AbilityButtonState.Active);
            _headButton.SetState(AbilityButtonState.Active);
            _dodgeButton.SetState(AbilityButtonState.Active);

            _handJoystic.SetState(AbilityButtonState.Active);
            _footJoystic.SetState(AbilityButtonState.Active);
        }
        #endregion

        #region METHODS PUBLIC
        public override void ShowScreen(object payload = null)
        {
            base.ShowScreen();
            ActivateButtonStates();
        }
        #endregion
    }
}

