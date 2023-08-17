using System;
using System.Collections.Generic;
using UnityEngine;
using EventHolder;
using Services.Database;
using Services.SaveSystem;
using Utility.DependencyInjection;

namespace Gameplay.Managers
{
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
            Action<float> eventCallback = (float delta) => { 
                EventHolder<StrengthChangeInfo>.NotifyListeners(new(_statLevels[statType], delta)); 
            };
            Action<uint> walletCallback = (uint cost) => {
                _walletService.TryGetCurrency<StrengthPointsDeposite>(cost);
            };

            StatPointsHandler(statType, info.Value, eventCallback, walletCallback);
        }

        [EventHolder]
        private void DexterityPointsChange(DexterityPointsChangeInfo info)
        {
            var statType = StatType.Dexterity;
            Action<float> eventCallback = (float delta) => {
                EventHolder<DexterityChangeInfo>.NotifyListeners(new(_statLevels[statType], delta));
            };
            Action<uint> walletCallback = (uint cost) => {
                _walletService.TryGetCurrency<DexterityPointsDeposite>(cost);
            };

            StatPointsHandler(statType, info.Value, eventCallback, walletCallback);
        }

        [EventHolder]
        private void EndurancePointsChange(EndurancePointsChangeInfo info)
        {
            var statType = StatType.Endurance;
            Action<float> eventCallback = (float delta) => {
                EventHolder<EnduranceChangeInfo>.NotifyListeners(new(_statLevels[statType], delta));
            };
            Action<uint> walletCallback = (uint cost) => {
                _walletService.TryGetCurrency<EndurancePointsDeposite>(cost);
            };

            StatPointsHandler(statType, info.Value, eventCallback, walletCallback);
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

        private void StatPointsHandler(StatType type, uint value, Action<float> eventCallback, Action<uint> walletCallback)
        {
            var cost = GetData(type).Cost;
            if (cost <= value)
            {
                _statLevels[type]++;
                SaveData();

                eventCallback.Invoke(CreateDelta(value, cost));
                walletCallback.Invoke(cost);
            }
            else
            {
                eventCallback.Invoke(CreateDelta(value, cost));
            }
        }

        private float CreateDelta(uint current, uint max)
        {
            return Mathf.Clamp((float)current / max, 0f, 1f);
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
