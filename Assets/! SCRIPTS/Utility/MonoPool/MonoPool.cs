using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utility.MonoPool
{
    public static class MonoPool
    {
        #region FIELDS PRIVATE
        private static Dictionary<MonoBehaviour, Stack<MonoBehaviour>> _monoPools = new();
        private static Transform _holder;
        #endregion

        #region PROPERTIES
        public static Transform Holder => _holder;
        #endregion

        #region METHODS PRIVATE
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            _holder = new GameObject($"===== MONO POOL =====").transform;
            Object.DontDestroyOnLoad(_holder);
        }
        #endregion

        #region METHODS PUBLIC
        public static T Instantiate<T>(T prefab) where T : MonoBehaviour
        {
            if (!_monoPools.ContainsKey(prefab))
            {
                _monoPools.Add(prefab, new Stack<MonoBehaviour>());
            }

            var monoStack = _monoPools[prefab];

            if (monoStack.Count == 0)
            {
                var mono = GameObject.Instantiate<T>(prefab);
                if(mono.TryGetComponent<MonoPoolComponent>(out var poolComponent))
                {
                    poolComponent.MonoStack = monoStack;
                }
                else
                {
                    mono.gameObject.AddComponent<MonoPoolComponent>().MonoStack = monoStack;
                }
                return mono;
            }
            else
            {
                var mono = monoStack.Pop() as T;
                mono.gameObject.transform.parent = null;
                mono.gameObject.SetActive(true);
                return mono;
            }
        }

        public static void Return<T>(T mono) where T : MonoBehaviour
        {
            if(mono.gameObject.TryGetComponent<MonoPoolComponent>(out var poolComponent))
            {
                poolComponent.Return<T>();
            }
            else
            {
                Debug.LogWarning("object not has mono pool component!", mono);
                GameObject.Destroy(mono.gameObject);
            }
        }
        #endregion
    }
}
