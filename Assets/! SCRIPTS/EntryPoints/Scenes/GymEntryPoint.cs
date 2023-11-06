using UnityEngine;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
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
            //TODO:
            DependencyContainer.Get<ISignalService>().Send<ShowScreen>(new(ScreenType.GymHUD));
        }
        #endregion
    }
}
