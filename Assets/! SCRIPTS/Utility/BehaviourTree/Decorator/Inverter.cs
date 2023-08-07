namespace Utility.BehaviourTree
{
    public class Inverter : Node
    {
        #region CONSTRUCTORS
        public Inverter(Node child) : base(new() { child }) { }
        #endregion

        #region METHODS PUBLIC
        public override NodeState Evaluate()
        {
            switch (_childs[0].Evaluate())
            {
                case NodeState.FAILURE:
                    _state = NodeState.SUCCESS;
                    break;
                case NodeState.SUCCESS:
                    _state = NodeState.FAILURE;
                    break;
                case NodeState.RUNNING:
                    _state = NodeState.RUNNING;
                    break;
            }

            return _state;
        }
        #endregion
    }
}
