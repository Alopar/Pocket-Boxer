using Services.SceneLoader;
using Utility.StateMachine;

namespace Services.Gameflow
{
    public partial class Gameflow
    {
        public abstract class AbstractGameflowState : AbstractState
        {
            #region FIELDS PRIVATE
            protected readonly ISceneLoaderService _sceneLoader;
            #endregion

            #region CONSTRUCTORS
            protected AbstractGameflowState(Gameflow entity, StateMachine stateMachine) : base(stateMachine)
            {
                _sceneLoader = entity._sceneLoader;
            }
            #endregion
        }
    }
}


