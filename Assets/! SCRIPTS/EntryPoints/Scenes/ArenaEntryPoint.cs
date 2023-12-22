using UnityEngine;
using Services.SignalSystem;
using Utility.DependencyInjection;
using Services.InputSystem;
using Services.ScreenSystem;
using Gameplay.Managers;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class ArenaEntryPoint : AbstractBootstrapper
    {
        #region METHODS PRIVATE
        protected override void RegisterDependencyContext()
        {
            DependencyContainer.Bind<FightManager>().AsSingle().NonLazy();
            DependencyContainer.Bind<BoxerFactory>();
        }

        protected override void InitializeScene()
        {
            //TODO: game state machine
            var screenService = DependencyContainer.Get<IScreenService>();
            var screenTypes = new ScreenType[] {
                ScreenType.ArenaHUD,
                ScreenType.Ability,
                ScreenType.Win,
                ScreenType.Lose
            };
            screenService.InitializeScreens(screenTypes);
            screenService.SetScreensCamera(Camera.main);

            screenService.ShowScreen(ScreenType.ArenaHUD);
            screenService.ShowScreen(ScreenType.Ability);

            DependencyContainer.Get<IInputService>().EnableInputs = InputType.None;
        }
        #endregion
    }
}
