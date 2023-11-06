using UnityEngine;
using Services.SignalSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class ArenaEntryPoint : AbstractSceneEntryPoint
    {
        #region METHODS PRIVATE
        protected override void RegisterDependencyContext()
        {

        }

        protected override void InitiateScene()
        {
            //TODO:
            //EventHolder<ShowScreenInfo>.Send(new(ScreenType.ArenaHUD));
        }
        #endregion
    }
}
