using System.Collections.Generic;

namespace Utility.BehaviourTree
{
    public abstract class Node
    {
        #region FIELDS PRIVATE
        private BehaviourTree _tree = null;
        private Node _parent = null;
        protected NodeState _state;
        protected List<Node> _childs = new();
        #endregion

        #region PROPERTIES
        public BehaviourTree Tree => _tree;
        public Node Parent => _parent;
        #endregion

        #region CONSTRUCTORS
        public Node() {}

        public Node(List<Node> childs)
        {
            childs.ForEach(e => Attach(e));
        }

        public Node(BehaviourTree tree, List<Node> childs) : this(childs)
        {
            SetTree(tree);
        }
        #endregion

        #region METHODS PRIVATE
        private void Attach(Node node)
        {
            node._parent = this;
            _childs.Add(node);
        }

        private void SetTree(BehaviourTree tree)
        {
            _tree = tree;
            _childs.ForEach(e => e.SetTree(_tree));
        }
        #endregion

        #region METHODS PUBLIC
        public abstract NodeState Evaluate();

        public T GetData<T>(string key)
        {
            return _tree.GetData<T>(key);
        }

        public void SetData(string key, object value)
        {
            _tree.SetData(key, value);
        }
        #endregion
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
}
