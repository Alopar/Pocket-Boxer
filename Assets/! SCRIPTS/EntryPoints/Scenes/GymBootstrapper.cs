using UnityEngine;
using Services.InputSystem;
using Services.ScreenSystem;
using Services.CurrencySystem;
using Utility.DependencyInjection;
using Services.TutorialSystem;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class GymBootstrapper : AbstractBootstrapper
    {
        #region FIELDS PRIVATE
        [Inject] private IInputService _inputService;
        [Inject] private IScreenService _screenService;
        [Inject] private ICurrencyService _currencyService;
        [Inject] private ITutorialService _tutorialService;
        #endregion

        #region METHODS PRIVATE
        protected override void InitializeScene()
        {
            InitializeScreens();
            ShowScreens();
            WarmupWallet();
            SwitchInputs();
            RiseTutorialEvent();
        }

        private void InitializeScreens()
        {
            var screenTypes = new ScreenType[] {
                ScreenType.Pointers,
                ScreenType.Inputters,
                ScreenType.GymHUD,
                ScreenType.Simulator,
                ScreenType.Relaxer,
                ScreenType.Fight
            };
            _screenService.ClearScreens();
            _screenService.InitializeScreens(screenTypes);
            _screenService.SetScreensCamera(Camera.main);
        }

        private void ShowScreens()
        {
            _screenService.ShowScreen(ScreenType.Pointers);
            _screenService.ShowScreen(ScreenType.Inputters);
            _screenService.ShowScreen(ScreenType.GymHUD);
        }

        private void WarmupWallet()
        {
            _currencyService.PutCurrency(CurrencyType.StrengthPoints, 0);
            _currencyService.PutCurrency(CurrencyType.DexterityPoints, 0);
            _currencyService.PutCurrency(CurrencyType.EndurancePoints, 0);
        }

        private void SwitchInputs()
        {
            _inputService.EnableInputs = InputType.Joystick;
        }

        private void RiseTutorialEvent()
        {
            _tutorialService.TriggerEvent(GameplayEvent.StartGame);
        }
        #endregion
    }
}
