using UnityEngine;
using Utility.GameSettings;

namespace Services.SaveSystem
{
    public class PlayerPrefSaveSystem : ISaveService
    {
        #region FIELDS PRIVATE
        private readonly SaveDataPreset _startPreset;
        private readonly SaveDataPreset _debugPreset;
        #endregion

        #region CONSTRUCTORS
        public PlayerPrefSaveSystem(SaveDataPreset startPreset, SaveDataPreset debugPreset)
        {
            _startPreset = startPreset;
            _debugPreset = debugPreset;
        }
        #endregion

        #region METHODS PUBLIC
        public void Save<T>(T data) where T : AbstractSaveData
        {
            var prefName = data.PrefName;
            var saveData = JsonUtility.ToJson(new DataWrapper<T>(data));
            PlayerPrefs.SetString(prefName, saveData);
            PlayerPrefs.Save();
        }

        public T Load<T>() where T : AbstractSaveData, new()
        {
#if DEBUG
            if (GameSettings.Data.UseDebugSaveData)
            {
                return _debugPreset.GetGameData<T>();
            }
#endif
            T result = new();
            var prefName = result.PrefName;
            if (PlayerPrefs.HasKey(prefName))
            {
                var loadData = PlayerPrefs.GetString(prefName);
                result = JsonUtility.FromJson<DataWrapper<T>>(loadData).Data;
            }
            else
            {
                result = _startPreset.GetGameData<T>();
            }

            return result;
        }
        #endregion
    }
}
