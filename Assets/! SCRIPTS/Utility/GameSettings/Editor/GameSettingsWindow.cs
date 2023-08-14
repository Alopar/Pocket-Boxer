using System.IO;
using UnityEngine;
using UnityEditor;

namespace Utility.GameSettings
{
    public class GameSettingsWindow : EditorWindow
    {
        #region FIELDS PRIVATE
        private const string TITLE_ICON_PATH = "GameSettingsWindow/titleIcon";
        private const string GAME_SETTINGS_PATH = "Assets/! SCRIPTS/Utility/GameSettings/Resources/game-settings.json";

        private static GameSettingsData _gameSettingsData;
        #endregion

        #region UNITY CALLBACKS
        private void OnGUI()
        {
            var windowData = new GameSettingsData();
            windowData.ApplicationFrameRate = EditorGUILayout.IntSlider("Frame rate:", _gameSettingsData.ApplicationFrameRate, 0, 60);
            windowData.UseDebugSaveData = EditorGUILayout.Toggle("Debug save data:", _gameSettingsData.UseDebugSaveData);
            windowData.ShowDebugMarkers = EditorGUILayout.Toggle("Show markers:", _gameSettingsData.ShowDebugMarkers);

            if (!windowData.Equals(_gameSettingsData))
            {
                _gameSettingsData = windowData;
                var json = JsonUtility.ToJson(_gameSettingsData);
                using (var writer = new StreamWriter(GAME_SETTINGS_PATH))
                { 
                    writer.Write(json);
                }
            }
        }

        private void OnFocus()
        {
            LoadData();
        }

        private void OnLostFocus()
        {
            AssetDatabase.ImportAsset(GAME_SETTINGS_PATH);
        }
        #endregion

        #region METHODS PRIVATE
        private static void Init()
        {
            LoadData();
        }

        private static void LoadData()
        {
            using (var reader = new StreamReader(GAME_SETTINGS_PATH))
            {
                var json = reader.ReadLine();
                _gameSettingsData = JsonUtility.FromJson<GameSettingsData>(json);
            }
        }

        [MenuItem("Tools/Utility/Game Settings")]
        private static void ShowWindow()
        {
            Init();
            var window = GetWindow<GameSettingsWindow>();
            var icon = Resources.Load<Texture>(TITLE_ICON_PATH);
            window.titleContent = new() { image = icon, text = "Game settings" };
        }
        #endregion
    }
}
