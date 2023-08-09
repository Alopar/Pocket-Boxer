using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.ScreenSystem
{
    public class ScreenSystem
    {
        #region FIELDS PRIVATE
        private readonly ScreenContainer _container;
        #endregion

        #region CONSTRUCTORS
        public ScreenSystem(ScreenContainer container)
        {
            _container = container;
            Init();
        }
        #endregion

        #region HANDLERS
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {
            InitializeScreens();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        private void InitializeScreens()
        {
            var holder = new GameObject("======== UI ========");
            foreach (var prefab in _container.ScreenPrefabs)
            {
                GameObject.Instantiate(prefab, holder.transform).name = prefab.name;
            }
#if DEBUG
            GameObject.Instantiate(_container.MonitoringPrefab, holder.transform).name = _container.MonitoringPrefab.name;
#endif
        }
        #endregion
    }
}
