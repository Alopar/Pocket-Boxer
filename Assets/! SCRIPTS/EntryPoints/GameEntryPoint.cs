using UnityEngine;
using UnityEngine.AI;
using Gameplay;
using Gameplay.Managers;
using Services.Database;
using Services.SaveSystem;
using Services.AudioSystem;
using Services.ScreenSystem;
using Services.TutorialSystem;
using Utility.GameSettings;
using Utility.DependencyInjection;
using DG.Tweening;

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

        private static void InitializeSystems()
        {
            var prefab = Resources.Load("SYSTEMS");
            var systems = GameObject.Instantiate(prefab);
            systems.name = "===== SYSTEMS =====";

            Object.DontDestroyOnLoad(systems);
        }

        private static void RegisterDependencyContext()
        {
            var startDataPreset = Resources.Load<SaveDataPreset>(GameSettings.StartSaveDataPresetPath);
            var debugDataPreset = Resources.Load<SaveDataPreset>(GameSettings.DebugSaveDataPresetPath);
            DependencyContainer.Bind<SaveDataPreset>("start").FromInstance(startDataPreset);
            DependencyContainer.Bind<SaveDataPreset>("debug").FromInstance(debugDataPreset);
            DependencyContainer.Bind<ISaveService>().To<PlayerPrefSaveSystem>().AsSingle();

            DependencyContainer.Bind<IDatabaseService>().To<ScriptableObjectDatabase>().AsSingle();
            DependencyContainer.Bind<IAudioService>().To<AudioSystem>().AsSingle();
            DependencyContainer.Bind<IWalletService>().To<Wallet>().AsSingle();

            var screenContainer = Resources.Load<ScreenContainer>(GameSettings.ScreenContainerPath);
            DependencyContainer.Bind<ScreenContainer>().FromInstance(screenContainer);
            DependencyContainer.Bind<ScreenSystem>().AsSingle().NonLazy();

            var tutorialSequence = Resources.Load<TutorialSequence>(GameSettings.TutorialSequencePath);
            DependencyContainer.Bind<TutorialSequence>().FromInstance(tutorialSequence);
            DependencyContainer.Bind<TutorialSystem>().AsSingle().NonLazy();

            DependencyContainer.Bind<StatsManager>().AsSingle().NonLazy();
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
