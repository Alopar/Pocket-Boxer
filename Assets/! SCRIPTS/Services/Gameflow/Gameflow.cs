using Services.SceneLoader;
using Utility.StateMachine;

namespace Services.Gameflow
{
    public partial class Gameflow
    {
        #region FIELDS PRIVATE
        private readonly StateMachine _stateMachine = new();
        private readonly ISceneLoaderService _sceneLoader;
        #endregion

        #region CONSTRUCTORS
        public Gameflow(ISceneLoaderService sceneLoader)
        {
            _sceneLoader = sceneLoader;
            InitializeStateMachine();
        }
        #endregion

        #region FIELDS PRIVATE
        private void InitializeStateMachine()
        {
            _stateMachine.Initialization(new() {
                new BootstrapGameflowState(this, _stateMachine),
            });
        }
        #endregion

    }
}
