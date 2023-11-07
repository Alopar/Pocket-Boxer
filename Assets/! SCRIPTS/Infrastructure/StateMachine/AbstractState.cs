namespace Utility.StateMachine
{
    public abstract class AbstractState : IState
    {
        protected readonly StateMachine _stateMachine;

        protected AbstractState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Enter() {}
        public virtual void Exit() {}
    }
}
