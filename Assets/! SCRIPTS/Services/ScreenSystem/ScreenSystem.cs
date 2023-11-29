using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utility.DependencyInjection;

namespace Services.ScreenSystem
{
    public class ScreenSystem : IScreenService
    {
        #region FIELDS PRIVATE
        [Inject] private readonly ScreenFactory _factory;
        private readonly ScreenContainer _container;

        private Transform _holder;
        private Dictionary<ScreenType, AbstractScreen> _screens = new();

        private bool _isFrameCounterInitialized = false;
        #endregion

        #region EVENTS
        public event Action<ScreenType> OnScreenOpen;
        public event Action<ScreenType> OnScreenClose;
        #endregion

        #region CONSTRUCTORS
        public ScreenSystem(ScreenContainer container)
        {
            _container = container;
        }
        #endregion

        #region METHODS PRIVATE
        private void InitializeHolder()
        {
            if (_holder != null) return;
            _holder = new GameObject("[Screens]").transform;
            GameObject.DontDestroyOnLoad(_holder);
        }

        private void InitializeFrameCounter(Transform holder)
        {
            if(_isFrameCounterInitialized) return;
            GameObject.Instantiate(_container.MonitoringPrefab, holder).name = _container.MonitoringPrefab.name;
            _isFrameCounterInitialized = true;
        }

        private bool CheckAvailableScreen(ScreenType screen)
        {
            if (!_screens.ContainsKey(screen))
            {
                Debug.LogError($"Screen with type {screen} not initialized!");
                return false;
            }

            return true;
        }
        #endregion

        #region METHODS PUBLIC
        public void InitializeScreens(ScreenType[] screenTypes)
        {
            InitializeHolder();
            foreach (var screenType in screenTypes)
            {
                var prefab = _container.ScreenPrefabs.FirstOrDefault(e => e.ScreenType == screenType);
                if (prefab == null)
                {
                    Debug.LogError($"Screen prefab with type {screenType} not found!");
                    continue;
                }

                var screen = _factory.Create(prefab);
                screen.transform.parent = _holder;
                screen.name = prefab.name;
                screen.HideScreen();

                _screens.Add(screenType, screen);
            }

#if DEBUG
            InitializeFrameCounter(_holder);
#endif
        }

        public void SetScreensCamera(Camera camera)
        {
            foreach (var screen in _screens.Values)
            {
                screen.SetCanvasCamera(camera);
            }
        }

        public void ShowScreen(ScreenType screen, object payload = null)
        {
            if (!CheckAvailableScreen(screen)) return;

            _screens[screen].ShowScreen(payload);
            OnScreenOpen?.Invoke(screen);
        }

        public void CloseScreen(ScreenType screen)
        {
            if (!CheckAvailableScreen(screen)) return;

            _screens[screen].CloseScreen();
            OnScreenClose?.Invoke(screen);
        }

        public void ClearScreens()
        {
            foreach (var screen in _screens.Values)
            {
                GameObject.Destroy(screen.gameObject);
            }

            _screens.Clear();
        }
        #endregion
    }
}
