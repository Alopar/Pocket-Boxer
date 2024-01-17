using System;

namespace Utility.GameSettings
{
    public static class GameSettings
    {
        #region FIELDS PRIVATE
        private const string START_SAVE_DATA_PRESET_PATH = "StartSaveData";
        private const string DEBUG_SAVE_DATA_PRESET_PATH = "DebugSaveData";
        private const string TUTORIAL_SEQUENCE_PATH = "TutorialSequence";
        private const string SCREEN_CONTAINER_PATH = "ScreenContainer";

        private static GameSettingsData _data;
        #endregion

        #region PROPERTIES
        public static string StartSaveDataPresetPath => START_SAVE_DATA_PRESET_PATH;
        public static string DebugSaveDataPresetPath => DEBUG_SAVE_DATA_PRESET_PATH;
        public static string TutorialSequencePath => TUTORIAL_SEQUENCE_PATH;
        public static string ScreenContainerPath => SCREEN_CONTAINER_PATH;

        public static GameSettingsData Data => _data;
        #endregion

        #region METHODS PUBLIC
        public static void Init(GameSettingsData data)
        {
            _data = data;
        }
        #endregion
    }

    [Serializable]
    public struct GameSettingsData
    {
        public int ApplicationFrameRate;
        public bool UseDebugSaveData;
        public bool ShowFrameCounter;
        public bool ShowDebugMarkers;
        public bool ShowDebugConsole;
    }
}