using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Gameplay;
using Gameplay.Managers;
using Services.Database;
using Services.SaveSystem;
using Services.InputSystem;
using Services.AudioSystem;
using Services.SignalSystem;
using Services.ScreenSystem;
using Services.AssetProvider;
using Services.PointerSystem;
using Services.CurrencySystem;
using Services.TutorialSystem;
using Utility.GameSettings;
using Utility.DependencyInjection;
using Container = Utility.DependencyInjection.DependencyContainer;

namespace Manager
{
    public static class GameEntryPoint
    {
        #region FIELDS PRIVATE
        private const string GAME_SETTINGS_NAME = "game-settings";
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
            Container.Bind<ComponentResolver>().AsSingle();
            Container.Bind<IAssetService>().To<ResourcesAssetProvider>().AsSingle();
            Container.Bind<ISignalService>().FromInstance(new SignalSystem(new EventBus()));

            BindSaveService();

            Container.Bind<IInputService>().To<InputSystem>().AsSingle();
            Container.Bind<IAudioService>().To<AudioSystem>().AsSingle();
            Container.Bind<IPointerService>().To<PointerSystem>().AsSingle();
            Container.Bind<ICurrencyService>().To<CurrencySystem>().AsSingle();
            Container.Bind<IDatabaseService>().To<ScriptableObjectDatabase>().AsSingle();

            BindScreenService();
            Container.Bind<ITutorialService>().To<TutorialSystem>().AsSingle();

            Container.Bind<StatsManager>().AsSingle().NonLazy();
            Container.Bind<GameTracker>().AsSingle().NonLazy();

            //TODO: refactoring
            UpdateCurrencyDeposites();
            Container.Get<ITutorialService>().TriggerEvent(GameplayEvent.StartGame);
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

        private static void InitializeSystems()
        {
            var prefab = Resources.Load("SYSTEMS");
            var systems = GameObject.Instantiate(prefab);
            systems.name = "===== SYSTEMS =====";

            Object.DontDestroyOnLoad(systems);
        }

        private static void InitializeOtherSystems()
        {
            DOTween.Init();
            NavMesh.avoidancePredictionTime = 0.5f;
            Application.targetFrameRate = GameSettings.Data.ApplicationFrameRate;
        }

        private static void UpdateCurrencyDeposites()
        {
            var currencyService = Container.Get<ICurrencyService>();
            currencyService.PutCurrency(CurrencyType.StrengthPoints, 0);
            currencyService.PutCurrency(CurrencyType.DexterityPoints, 0);
            currencyService.PutCurrency(CurrencyType.EndurancePoints, 0);
        }
        #endregion

        #region METHODS PUBLIC
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void GameInitialize()
        {
            InitializeGameSettings();
            RegisterDependencyContext();

            InitializeSystems();
            InitializeOtherSystems();
        }
        #endregion
    }
}
