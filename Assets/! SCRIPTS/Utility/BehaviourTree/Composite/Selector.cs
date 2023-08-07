using System.Collections.Generic;

namespace Utility.BehaviourTree
{
    public class Selector : Node
    {
        #region CONSTRUCTORS
        public Selector(List<Node> children) : base(children) { }
        #endregion

        #region METHODS PUBLIC
        public override NodeState Evaluate()
        {
            foreach (var node in _childs)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        _state = NodeState.SUCCESS;
                        return _state;
                    case NodeState.RUNNING:
                        _state = NodeState.RUNNING;
                        return _state;
                }
            }

            _state = NodeState.FAILURE;
            return _state;
        }
        #endregion
    }
}
