namespace Utility.BehaviourTree
{
    public class ActionNode : Node
    {
        #region FIELDS PRIVATE
        private ActionNodeDelegate _action;
        #endregion

        #region CONSTRUCTORS
        public ActionNode(ActionNodeDelegate action)
        {
            _action = action;
        }
        #endregion

        #region METHODS PUBLIC
        public override NodeState Evaluate()
        {
            _state = _action(this);
            return _state;
        }
        #endregion
    }

    public delegate NodeState ActionNodeDelegate(Node node);
}