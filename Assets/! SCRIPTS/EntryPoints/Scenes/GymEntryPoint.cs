using UnityEngine;
using Utility.DependencyInjection;
using Services.InputSystem;
using Services.ScreenSystem;
using Services.CurrencySystem;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class GymEntryPoint : AbstractBootstrapper
    {
        #region METHODS PRIVATE
        protected override void RegisterDependencyContext()
        {
            DependencyContainer.Bind<PlayerFactory>();
            DependencyContainer.Bind<TokenFactory>();
        }

        protected override void InitializeScene()
        {
            //TODO: game state machine
            var screenService = DependencyContainer.Get<IScreenService>();
            var screenTypes = new ScreenType[] {
                ScreenType.Pointers,
                ScreenType.Inputters,
                ScreenType.GymHUD,
                ScreenType.Simulator,
                ScreenType.Relaxer
            };
            screenService.InitializeScreens(screenTypes);
            screenService.SetScreensCamera(Camera.main);

            screenService.ShowScreen(ScreenType.Pointers);
            screenService.ShowScreen(ScreenType.Inputters);
            screenService.ShowScreen(ScreenType.GymHUD);

            DependencyContainer.Get<IInputService>().EnableInputs = InputType.Joystick;

            var currencyService = DependencyContainer.Get<ICurrencyService>();
            currencyService.PutCurrency(CurrencyType.StrengthPoints, 0);
            currencyService.PutCurrency(CurrencyType.DexterityPoints, 0);
            currencyService.PutCurrency(CurrencyType.EndurancePoints, 0);
        }
        #endregion
    }
}
