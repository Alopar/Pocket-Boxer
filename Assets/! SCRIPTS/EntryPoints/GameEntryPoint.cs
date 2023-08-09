using UnityEngine;
using UnityEngine.AI;
using Gameplay;
using Services.Database;
using Services.SaveSystem;
using Services.AudioSystem;
using Services.ServiceLocator;
using Utility;
using DG.Tweening;
using Services.TutorialSystem;

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

            var tutorialSequence = Resources.Load<TutorialSequence>(GameSettings.TutorialSequencePath);
            ServiceLocator.RegisterService(new TutorialSystem(tutorialSequence, ServiceLocator.GetService<ISaveService>()));
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

            InitializeManagers();
            InitializeOtherSystems();
        }
        #endregion
    }
}
