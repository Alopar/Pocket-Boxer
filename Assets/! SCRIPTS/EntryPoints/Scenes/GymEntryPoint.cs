using UnityEngine;
using Services.SignalSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class GymEntryPoint : AbstractSceneEntryPoint
    {
        #region METHODS PRIVATE
        protected override void RegisterDependencyContext()
        {
            DependencyContainer.Bind<PlayerFactory>();
            DependencyContainer.Bind<TokenFactory>();
        }

        protected override void InitiateScene()
        {
            SignalSystem<ShowScreenInfo>.Send(new(ScreenType.GymHUD));
        }
        #endregion
    }
}
