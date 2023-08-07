using System.Collections.Generic;
using UnityEngine;

namespace Utility.BehaviourTree
{
    public class BehaviourTree : MonoBehaviour
    {
        #region METHODS PRIVATE
        private Root _root = null;
        private Dictionary<string, object> _blackboard = new();
        #endregion

        #region UNITY CALLBACKS
        private void FixedUpdate()
        {
            if (_root is not null)
            {
                _root.Evaluate();
            }
        }
        #endregion

        #region METHODS PRIVATE
        public void SetupTree(Root root)
        {
            _root = root;
        }

        public T GetData<T>(string key)
        {
            if (_blackboard.ContainsKey(key))
            {
                return (T)_blackboard[key];
            }

            return default;
        }

        public void SetData(string key, object value)
        {
            _blackboard[key] = value;
        }
        #endregion
    }
}