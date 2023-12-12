using Utility.StateMachine;
using static Services.Gameflow.Gameflow;

namespace Services.Gameflow
{
    public class BootstrapGameflowState : AbstractGameflowState
    {
        #region CONSTRUCTORS
        public BootstrapGameflowState(Gameflow entity, StateMachine stateMachine) : base(entity, stateMachine) { }
        #endregion

        #region METHODS PUBLIC
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }
        #endregion
    }
}
