using System;

namespace Utility
{
    public static class GameSettings
    {
        #region FIELDS PRIVATE
        private static GameSettingsData _data;
        #endregion

        #region PROPERTIES
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
        public bool ShowDebugMarkers;
    }
}