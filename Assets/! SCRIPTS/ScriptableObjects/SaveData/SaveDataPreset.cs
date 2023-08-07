using UnityEngine;
using Manager;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "NewSaveDataPreset", menuName = "Presets/SaveDataPreset", order = 0)]
    public class SaveDataPreset : ScriptableObject
    {
        #region FIELDS INSPECTOR
        [SerializeField] private CurrencyData _currencyData;
        [SerializeField] private TutorialData _tutorialData;
        [SerializeField] private UpgradeData _upgradeData;
        #endregion

        #region FIELDS PRIVATE
        private List<BaseGameData> _gameDatas;
        #endregion

        #region METHODS PRIVATE
        private List<BaseGameData> CreateGameDatas()
        {
            var gameDatas = new List<BaseGameData>
            {
                _currencyData,
                _tutorialData,
                _upgradeData,
            };

            return gameDatas;
        }
        #endregion

        #region METHODS PUBLIC
        public T GetGameData<T>() where T : BaseGameData
        {
            _gameDatas ??= CreateGameDatas();
            return _gameDatas.OfType<T>().First().Copy<T>();
        }
        #endregion

    }
}