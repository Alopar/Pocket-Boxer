using UnityEngine;
using UnityEngine.SceneManagement;
using Gameplay;
using System.Collections.Generic;

namespace Manager
{
    [DefaultExecutionOrder(-10)]
    public class UiManger : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<AScreenUiController> _screenPrefabs;

        [Space(10)]
        [SerializeField] private Tayx.Graphy.GraphyManager _monitoringPrefab;
        #endregion

        #region FIELDS PRIVATE
        private GameObject _base;
        #endregion

        #region HANDLERS
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {
            _base = new GameObject("======== UI ========");
            foreach (var prefab in _screenPrefabs)
            {
                Instantiate(prefab, _base.transform).name = prefab.name;
            }
#if DEBUG
            Instantiate(_monitoringPrefab, _base.transform).name = _monitoringPrefab.name;
#endif
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }
        #endregion
    }
}
