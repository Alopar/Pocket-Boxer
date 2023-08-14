using UnityEngine;
using UnityEngine.AI;
using Gameplay;
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

        private static void InitializeManagers()
        {
            var prefab = Resources.Load("SYSTEMS");
            var managers = GameObject.Instantiate(prefab);
            managers.name = "===== MANAGERS =====";

            Object.DontDestroyOnLoad(managers);
        }

        private static void RegisterDependencyContext()
        {
            var startDataPreset = Resources.Load<SaveDataPreset>(GameSettings.StartSaveDataPresetPath);
            var debugDataPreset = Resources.Load<SaveDataPreset>(GameSettings.DebugSaveDataPresetPath);
            DependenciesContext.Bind<SaveDataPreset>("start").FromInstance(startDataPreset);
            DependenciesContext.Bind<SaveDataPreset>("debug").FromInstance(debugDataPreset);
            DependenciesContext.Bind<ISaveService>().To<PlayerPrefSaveSystem>().AsSingle();

            DependenciesContext.Bind<IDatabaseService>().To<ScriptableObjectDatabase>().AsSingle();
            DependenciesContext.Bind<IAudioService>().To<AudioSystem>().AsSingle();
            DependenciesContext.Bind<IWalletService>().To<Wallet>().AsSingle();

            var screenContainer = Resources.Load<ScreenContainer>(GameSettings.ScreenContainerPath);
            DependenciesContext.Bind<ScreenContainer>().FromInstance(screenContainer);
            DependenciesContext.Bind<ScreenSystem>().AsSingle().NonLazy();

            var tutorialSequence = Resources.Load<TutorialSequence>(GameSettings.TutorialSequencePath);
            DependenciesContext.Bind<TutorialSequence>().FromInstance(tutorialSequence);
            DependenciesContext.Bind<TutorialSystem>().AsSingle().NonLazy();
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

            InitializeManagers();
            InitializeOtherSystems();
        }
        #endregion
    }
}
