using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Services.SaveSystem
{
    [CreateAssetMenu(fileName = "NewSaveDataPreset", menuName = "Presets/SaveDataPreset", order = 0)]
    public class SaveDataPreset : ScriptableObject
    {
        #region FIELDS INSPECTOR
        [SerializeField] private CurrencySaveData _currencyData;
        [SerializeField] private TutorialSaveData _tutorialData;
        [SerializeField] private UpgradeSaveData _upgradeData;
        [SerializeField] private BuildingSaveData _buildingData;
        [SerializeField] private ArmorySaveData _armoryData;
        [SerializeField] private LevelSaveData _levelData;
        #endregion

        #region FIELDS PRIVATE
        private List<AbstractSaveData> _gameDatas;
        #endregion

        #region METHODS PRIVATE
        private List<AbstractSaveData> CreateGameDatas()
        {
            var result = new List<AbstractSaveData>();

            var type = typeof(SaveDataPreset);
            var flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic;
            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                if (!CheckFieldAttributesOnType<SerializeField>(field)) continue;
                if (!CheckTypeToBaseType<AbstractSaveData>(field.FieldType)) continue;

                result.Add(field.GetValue(this) as AbstractSaveData);
            }

            return result;
        }

        private bool CheckFieldAttributesOnType<T>(FieldInfo field)
        {
            var result = false;
            var attributes = field.GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                result = attribute.GetType() == typeof(T) ? true : result;
            }

            return result;
        }

        private bool CheckTypeToBaseType<T>(Type type)
        {
            var result = false;
            var parrentType = type.BaseType;
            while (parrentType is not null)
            {
                result = parrentType == typeof(T) ? true : result;
                parrentType = parrentType.BaseType;
            }

            return result;
        }
        #endregion

        #region METHODS PUBLIC
        public T GetGameData<T>() where T : AbstractSaveData
        {
            _gameDatas ??= CreateGameDatas();
            return _gameDatas.OfType<T>().First().Copy<T>();
        }
        #endregion
    }
}
