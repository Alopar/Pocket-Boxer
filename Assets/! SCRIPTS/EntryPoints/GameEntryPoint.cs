using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using Gameplay;
using Gameplay.Managers;
using Services.Database;
using Services.SaveSystem;
using Services.AudioSystem;
using Services.SignalSystem;
using Services.ScreenSystem;
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
            Container.Bind<ComponentDependencyResolver>().AsSingle();

            Container.Bind<ISignalService>().FromInstance(new SignalSystem(new EventBus()));

            var startDataPreset = Resources.Load<SaveDataPreset>(GameSettings.StartSaveDataPresetPath);
            var debugDataPreset = Resources.Load<SaveDataPreset>(GameSettings.DebugSaveDataPresetPath);
            Container.Bind<SaveDataPreset>("start").FromInstance(startDataPreset);
            Container.Bind<SaveDataPreset>("debug").FromInstance(debugDataPreset);
            Container.Bind<ISaveService>().To<PlayerPrefSaveSystem>().AsSingle();

            Container.Bind<IDatabaseService>().To<ScriptableObjectDatabase>().AsSingle();
            Container.Bind<IAudioService>().To<AudioSystem>().AsSingle();
            Container.Bind<IWalletService>().To<Wallet>().AsSingle();
            Container.Bind<GameTracker>().AsSingle().NonLazy();

            var screenContainer = Resources.Load<ScreenContainer>(GameSettings.ScreenContainerPath);
            Container.Bind<ScreenContainer>().FromInstance(screenContainer);
            Container.Bind<ScreenFactory>();
            Container.Bind<ScreenSystem>().AsSingle().NonLazy();

            var tutorialSequence = Resources.Load<TutorialSequence>(GameSettings.TutorialSequencePath);
            Container.Bind<TutorialSequence>().FromInstance(tutorialSequence);
            Container.Bind<TutorialSystem>().AsSingle().NonLazy();

            Container.Bind<StatsManager>().AsSingle().NonLazy();
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
