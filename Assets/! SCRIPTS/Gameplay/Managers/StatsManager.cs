using System;
using System.Collections.Generic;
using EventHolder;
using Services.Database;
using Services.SaveSystem;
using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay.Managers
{
    //TODO: refactoring handlers!
    public class StatsManager
    {
        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;
        [Inject] private IWalletService _walletService;
        [Inject] private IDatabaseService _databaseService;

        private Dictionary<StatType, ushort> _statLevels = new();
        private Dictionary<StatType, StatsUpgrade> _statUpgradeTables = new();
        #endregion

        #region CONSTRUCTORS
        public StatsManager()
        {
            LoadData();
            TakeDatabaseTables();

            SubscribeService.SubscribeListener(this);
        }
        #endregion

        #region HANDLERS
        [EventHolder]
        private void StrengthPointsChange(StrengthPointsChangeInfo info)
        {
            var statType = StatType.Strength;
            var statData = GetData(statType);
            if(statData.Cost <= info.Value)
            {
                _statLevels[StatType.Strength]++;
                SaveData();

                var delta = Mathf.Clamp((float)info.Value / statData.Cost, 0f, 1f);
                EventHolder<StatsChangeInfo>.NotifyListeners(new(statType, _statLevels[statType], delta));

                _walletService.TryGetCurrency<StrengthPointsDeposite>(statData.Cost);
            }
            else
            {
                var delta = Mathf.Clamp((float)info.Value / statData.Cost, 0f, 1f);
                EventHolder<StatsChangeInfo>.NotifyListeners(new(statType, _statLevels[statType], delta));
            }
        }

        [EventHolder]
        private void DexterityPointsChange(DexterityPointsChangeInfo info)
        {
            var statType = StatType.Dexterity;
            var statData = GetData(statType);
            if (statData.Cost <= info.Value)
            {
                _statLevels[statType]++;
                SaveData();

                var delta = Mathf.Clamp((float)info.Value / statData.Cost, 0f, 1f);
                EventHolder<StatsChangeInfo>.NotifyListeners(new(statType, _statLevels[statType], delta));

                _walletService.TryGetCurrency<DexterityPointsDeposite>(statData.Cost);
            }
            else
            {
                var delta = Mathf.Clamp((float)info.Value / statData.Cost, 0f, 1f);
                EventHolder<StatsChangeInfo>.NotifyListeners(new(statType, _statLevels[statType], delta));
            }
        }

        [EventHolder]
        private void EndurancePointsChange(EndurancePointsChangeInfo info)
        {
            var statType = StatType.Endurance;
            var statData = GetData(statType);
            if (statData.Cost <= info.Value)
            {
                _statLevels[statType]++;
                SaveData();

                var delta = Mathf.Clamp((float)info.Value / statData.Cost, 0f, 1f);
                EventHolder<StatsChangeInfo>.NotifyListeners(new(statType, _statLevels[statType], delta));

                _walletService.TryGetCurrency<EndurancePointsDeposite>(statData.Cost);
            }
            else
            {
                var delta = Mathf.Clamp((float)info.Value / statData.Cost, 0f, 1f);
                EventHolder<StatsChangeInfo>.NotifyListeners(new(statType, _statLevels[statType], delta));
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void LoadData()
        {
            var loadData = _saveService.Load<StatsSaveData>();
            _statLevels[StatType.Strength] = loadData.StrengthLevel;
            _statLevels[StatType.Dexterity] = loadData.DexterityLevel;
            _statLevels[StatType.Endurance] = loadData.EnduranceLevel;
        }

        private void SaveData()
        {
            var saveData = new StatsSaveData();
            saveData.StrengthLevel = _statLevels[StatType.Strength];
            saveData.DexterityLevel = _statLevels[StatType.Dexterity];
            saveData.EnduranceLevel = _statLevels[StatType.Endurance];

            _saveService.Save(saveData);
        }

        private void TakeDatabaseTables()
        {
            var db = _databaseService;
            _statUpgradeTables[StatType.Strength] = db.GetTable<StatsUpgradeData>("StrengthStatsUpgrade") as StatsUpgrade;
            _statUpgradeTables[StatType.Dexterity] = db.GetTable<StatsUpgradeData>("DexterityStatsUpgrade") as StatsUpgrade;
            _statUpgradeTables[StatType.Endurance] = db.GetTable<StatsUpgradeData>("EnduranceStatsUpgrade") as StatsUpgrade;
        }
        #endregion

        #region METHODS PUBLIC
        public int GetLevel(StatType type)
        {
            return _statLevels[type];
        }

        public StatsUpgradeData GetData(StatType type)
        {
            return _statUpgradeTables[type].GetDataByIndex((uint)GetLevel(type) - 1);
        }
        #endregion
    }
}
