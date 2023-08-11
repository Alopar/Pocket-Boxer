using UnityEngine;
using UnityEngine.AI;
using Gameplay;
using Services.Database;
using Services.SaveSystem;
using Services.AudioSystem;
using Services.ScreenSystem;
using Services.TutorialSystem;
using Services.ServiceLocator;
using Utility;
using DG.Tweening;
using Utility.DependencyInjection;

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

        private static void RegisterServices()
        {
            var startDataPreset = Resources.Load<SaveDataPreset>(GameSettings.StartSaveDataPresetPath);
            var debugDataPreset = Resources.Load<SaveDataPreset>(GameSettings.DebugSaveDataPresetPath);
            ServiceLocator.RegisterService<ISaveService>(new PlayerPrefSaveSystem(startDataPreset, debugDataPreset));

            ServiceLocator.RegisterService<IDatabaseService>(new ScriptableObjectDatabase());
            ServiceLocator.RegisterService<IWalletService>(new Wallet(ServiceLocator.GetService<ISaveService>()));
            ServiceLocator.RegisterService<IAudioService>(new AudioSystem());

            var screenContainer = Resources.Load<ScreenContainer>(GameSettings.ScreenContainerPath);
            ServiceLocator.RegisterService(new ScreenSystem(screenContainer));

            var tutorialSequence = Resources.Load<TutorialSequence>(GameSettings.TutorialSequencePath);
            ServiceLocator.RegisterService(new TutorialSystem(tutorialSequence, ServiceLocator.GetService<ISaveService>()));
        }

        private static void RegisterDependencyContext()
        {
            var startDataPreset = Resources.Load<SaveDataPreset>(GameSettings.StartSaveDataPresetPath);
            var debugDataPreset = Resources.Load<SaveDataPreset>(GameSettings.DebugSaveDataPresetPath);
            GameDependenciesContext.Bind<ISaveService>().FromInstance(new PlayerPrefSaveSystem(startDataPreset, debugDataPreset));

            GameDependenciesContext.Bind<IDatabaseService>().To<ScriptableObjectDatabase>().AsSingle();
            GameDependenciesContext.Bind<IAudioService>().To<AudioSystem>().AsSingle();

            var screenContainer = Resources.Load<ScreenContainer>(GameSettings.ScreenContainerPath);
            GameDependenciesContext.Bind<ScreenSystem>().FromInstance(new ScreenSystem(screenContainer));

            //var tutorialSequence = Resources.Load<TutorialSequence>(GameSettings.TutorialSequencePath);
            //GameDependenciesContext.Bind<TutorialSystem>().FromInstance()
            //ServiceLocator.RegisterService(new TutorialSystem(tutorialSequence, ServiceLocator.GetService<ISaveService>()));
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
            RegisterServices();
            RegisterDependencyContext();

            InitializeManagers();
            InitializeOtherSystems();
        }
        #endregion
    }
}
