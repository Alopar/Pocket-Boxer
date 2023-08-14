using UnityEngine;
using Utility.GameSettings;
using Utility.DependencyInjection;

namespace Services.SaveSystem
{
    public class PlayerPrefSaveSystem : ISaveService
    {
        #region FIELDS PRIVATE
        [Inject("start")] private SaveDataPreset _startPreset;
        [Inject("debug")] private SaveDataPreset _debugPreset;
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
