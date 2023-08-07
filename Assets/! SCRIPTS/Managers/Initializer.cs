using UnityEngine;
using Utility;

namespace Manager
{
    public static class Initializer
    {
        #region FIELDS PRIVATE
        private const string GAME_SETTINGS_NAME = "game-settings";
        #endregion

        #region METHODS PUBLIC
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeManagers()
        {
            var json = Resources.Load<TextAsset>(GAME_SETTINGS_NAME);
            var data = JsonUtility.FromJson<GameSettingsData>(json.text);
            GameSettings.Init(data);

            var prefab = Resources.Load("SYSTEMS");
            var managers = GameObject.Instantiate(prefab);
            managers.name = "===== MANAGERS =====";

            Object.DontDestroyOnLoad(managers);
        }
        #endregion
    }
}