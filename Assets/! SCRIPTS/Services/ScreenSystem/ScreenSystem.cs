using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.DependencyInjection;

namespace Services.ScreenSystem
{
    public class ScreenSystem : IScreenService
    {
        #region FIELDS PRIVATE
        [Inject] private readonly ScreenFactory _factory;

        private readonly ScreenContainer _container;
        #endregion

        #region CONSTRUCTORS
        public ScreenSystem(ScreenContainer container)
        {
            _container = container;

            SceneManager.sceneLoaded += SceneLoadedHandler;
        }
        #endregion

        #region HANDLERS
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {
            InitializeScreens();
        }
        #endregion

        #region METHODS PRIVATE
        private void InitializeScreens()
        {
            var holder = new GameObject("======== UI ========");
            foreach (var prefab in _container.ScreenPrefabs)
            {
                var screen = _factory.Create(prefab);
                screen.transform.parent = holder.transform;
                screen.name = prefab.name;
            }
#if DEBUG
            GameObject.Instantiate(_container.MonitoringPrefab, holder.transform).name = _container.MonitoringPrefab.name;
#endif
        }
        #endregion
    }
}
