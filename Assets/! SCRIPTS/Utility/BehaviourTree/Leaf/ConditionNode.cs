namespace Utility.BehaviourTree
{
    public class ConditionNode : Node
    {
        #region FIELDS PRIVATE
        private ConditionNodeDelegate _condition;
        #endregion

        #region CONSTRUCTORS
        public ConditionNode(ConditionNodeDelegate condition)
        {
            _condition = condition;
        }
        #endregion

        #region METHODS PUBLIC
        public override NodeState Evaluate()
        {
            _state = _condition(this) ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
        #endregion
    }

    public delegate bool ConditionNodeDelegate(Node node);
}