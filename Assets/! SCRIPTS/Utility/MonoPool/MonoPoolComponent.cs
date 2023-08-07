using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utility.MonoPool
{
    public class MonoPoolComponent : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private Stack<MonoBehaviour> _monoStack;
        #endregion

        #region PROPERTIES
        public Stack<MonoBehaviour> MonoStack
        {
            set
            {
                if (_monoStack == null)
                {
                    _monoStack = value;
                }
                else
                {
                    Debug.LogWarning("mono pool may set only one once!");
                }
            }
        }
        #endregion

        #region EVENTS
        public event UnityAction OnClear;
        #endregion

        #region METHODS PUBLIC
        public virtual void Clear()
        {
            transform.parent = null;
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;

            OnClear?.Invoke();
        }

        public void Return<T>() where T : MonoBehaviour
        {
            if (_monoStack == null)
            {
                Debug.LogError("mono object was not be instantiated!", this);
                return;
            }

            Clear();

            var mono = GetComponent<T>();
            _monoStack.Push(mono);
            gameObject.SetActive(false);

            if (TryGetComponent<RectTransform>(out var rectTransform))
            {
                rectTransform.SetParent(MonoPool.Holder, false);
            }
            else
            {
                transform.SetParent(MonoPool.Holder);
            }
        }
        #endregion
    }
}