using UnityEngine;
using UnityEngine.SceneManagement;
using Gameplay;
using Gameplay.Managers;
using Services.Database;
using Services.SaveSystem;
using Services.SceneLoader;
using Services.InputSystem;
using Services.AudioSystem;
using Services.SignalSystem;
using Services.ScreenSystem;
using Services.AssetProvider;
using Services.PointerSystem;
using Services.CurrencySystem;
using Services.TutorialSystem;
using Utility.GameSettings;
using Utility.CoroutineRunner;
using Utility.DependencyInjection;
using Container = Utility.DependencyInjection.DependencyContainer;

namespace Manager
{
    public static class GameEntryPoint
    {
        #region FIELDS PRIVATE
        private const string GAME_SETTINGS_NAME = "game-settings";
        private static int _startSceneIndex = 0;
        #endregion

        #region PROPERTIES
        public static int StartSceneIndex => _startSceneIndex;
        #endregion

        #region METHODS PRIVATE
        private static void InitializeGameSettings()
        {
            var json = Resources.Load<TextAsset>(GAME_SETTINGS_NAME);
            var data = JsonUtility.FromJson<GameSettingsData>(json.text);
            GameSettings.Init(data);
        }

        private static void RegisterDependencyContext()
        {
            BindCoroutineRunner();
            Container.Bind<ISceneLoaderService>().To<SceneLoader>().AsSingle();

            Container.Bind<ComponentResolver>().AsSingle();
            Container.Bind<IAssetService>().To<ResourcesAssetProvider>().AsSingle();
            Container.Bind<ISignalService>().FromInstance(new SignalSystem(new EventBus()));

            BindSaveService();

            Container.Bind<IInputService>().To<InputSystem>().AsSingle();
            Container.Bind<IAudioService>().To<AudioSystem>().AsSingle();
            Container.Bind<IPointerService>().To<PointerSystem>().AsSingle();
            Container.Bind<ICurrencyService>().To<CurrencySystem>().AsSingle();
            Container.Bind<IDatabaseService>().To<ScriptableObjectDatabase>().AsSingle();
            Container.Bind<ITutorialService>().To<TutorialSystem>().AsSingle();

            BindScreenService();

            Container.Bind<StatsManager>().AsSingle().NonLazy();
            Container.Bind<FightManager>().AsSingle().NonLazy();
            Container.Bind<GameTracker>().AsSingle().NonLazy();

            Container.Bind<TokenFactory>();
            Container.Bind<BoxerFactory>();
            Container.Bind<PlayerFactory>();
        }

        private static void BindCoroutineRunner()
        {
            var gameObject = new GameObject("[CoroutineRunner]");
            var coroutineRunner = gameObject.AddComponent<CoroutineRunner>();
            GameObject.DontDestroyOnLoad(coroutineRunner);

            Container.Bind<ICoroutineRunner>().FromInstance(coroutineRunner);
        }

        private static void BindSaveService()
        {
            var startPreset = Resources.Load<SaveDataPreset>(GameSettings.StartSaveDataPresetPath);
            var debugPreset = Resources.Load<SaveDataPreset>(GameSettings.DebugSaveDataPresetPath);
            Container.Bind<ISaveService>().FromInstance(new PlayerPrefSaveSystem(startPreset, debugPreset));
        }

        private static void BindScreenService()
        {
            var screenContainer = Resources.Load<ScreenContainer>(GameSettings.ScreenContainerPath);
            Container.Bind<ScreenFactory>();
            Container.Bind<IScreenService>().FromInstance(new ScreenSystem(screenContainer));
        }

        private static void LoadBootstrapScene()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.buildIndex == 0) return;

            _startSceneIndex = scene.buildIndex;
            SceneManager.LoadScene(0);
        }
        #endregion

        #region METHODS PUBLIC
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void GameInitialize()
        {
            InitializeGameSettings();
            RegisterDependencyContext();
            LoadBootstrapScene();
        }
        #endregion
    }
}
