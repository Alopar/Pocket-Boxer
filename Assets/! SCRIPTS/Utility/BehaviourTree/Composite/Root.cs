using System.Collections.Generic;

namespace Utility.BehaviourTree
{
    public class Root : Node
    {
        #region CONSTRUCTORS
        public Root(BehaviourTree tree, Node child) : base(tree, new() { child }) { }
        #endregion

        #region METHODS PUBLIC
        public override NodeState Evaluate()
        {
            return _childs[0].Evaluate();
        }
        #endregion
    }
}
