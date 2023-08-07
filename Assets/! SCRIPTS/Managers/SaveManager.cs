using System;
using UnityEngine;
using Gameplay;
using Utility;

namespace Manager
{
    [DefaultExecutionOrder(-20)]
    public class SaveManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private SaveDataPreset _startPreset;
        [SerializeField] private SaveDataPreset _debugPreset;
        #endregion

        #region FIELDS PRIVATE
        private static SaveManager _instance;
        #endregion

        #region PROPERTIES
        public static SaveManager Instance => _instance;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion

        #region METHODS PUBLIC
        public void Save<T>(T data) where T : BaseGameData
        {
            var prefName = data.PrefName;
            var saveData = JsonUtility.ToJson(new SaveData<T>(data));
            PlayerPrefs.SetString(prefName, saveData);
            PlayerPrefs.Save();
        }

        public T Load<T>() where T : BaseGameData, new()
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
                result = JsonUtility.FromJson<SaveData<T>>(loadData).Data;
            }
            else
            {
                result = _startPreset.GetGameData<T>();
            }

            return result;
        }
        #endregion
    }

    [Serializable]
    public class SaveData<T> where T : BaseGameData
    {
        public T Data;

        public SaveData(T data)
        {
            Data = data;
        }
    }

    public abstract class BaseGameData
    {
        public abstract string PrefName { get; }
        public abstract T Copy<T>() where T : BaseGameData;
    }
}