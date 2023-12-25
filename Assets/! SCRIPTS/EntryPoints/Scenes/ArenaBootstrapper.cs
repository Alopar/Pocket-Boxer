using UnityEngine;
using Services.InputSystem;
using Services.ScreenSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class ArenaBootstrapper : AbstractBootstrapper
    {
        #region FIELDS PRIVATE
        [Inject] private IInputService _inputService;
        [Inject] private IScreenService _screenService;
        #endregion

        #region METHODS PRIVATE
        protected override void InitializeScene()
        {
            InitializeScreens();
            ShowScreens();
            SwitchInputs();
        }

        private void InitializeScreens()
        {
            var screenTypes = new ScreenType[] {
                ScreenType.ArenaHUD,
                ScreenType.Ability,
                ScreenType.Win,
                ScreenType.Lose
            };
            _screenService.InitializeScreens(screenTypes);
            _screenService.SetScreensCamera(Camera.main);
        }

        private void ShowScreens()
        {
            _screenService.ShowScreen(ScreenType.ArenaHUD);
            _screenService.ShowScreen(ScreenType.Ability);
        }

        private void SwitchInputs()
        {
            _inputService.EnableInputs = InputType.None;
        }
        #endregion
    }
}
