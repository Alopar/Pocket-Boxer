using System.Collections.Generic;

namespace Utility.BehaviourTree
{
    public class Sequence : Node
    {
        #region CONSTRUCTORS
        public Sequence(List<Node> children) : base(children) { }
        #endregion

        #region METHODS PUBLIC
        public override NodeState Evaluate()
        {
            var anyChildIsRunning = false;
            foreach (var node in _childs)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        _state = NodeState.FAILURE;
                        return _state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                }
            }

            _state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return _state;
        }
        #endregion
    }
}
