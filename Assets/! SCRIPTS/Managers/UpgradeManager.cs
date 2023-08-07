using UnityEngine;
using Gameplay;
using EventHolder;
using System.Collections.Generic;

namespace Manager
{
    [DefaultExecutionOrder(-15)]
    public class UpgradeManager : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private static UpgradeManager _instance;

        private Dictionary<UpgradeType, int> _upgradeLevels = new();
        private Dictionary<UpgradeType, DatabaseTable> _upgradeTables = new();
        #endregion

        #region PROPERTIES
        public static UpgradeManager Instance => _instance;
        #endregion

        #region HANDLERS
        private void h_UpgradeIncrease(UpgradeIncreaseInfo info)
        {
            _upgradeLevels[info.UpgradeType]++;

            SaveData();
            EventHolder<UpgradeChangeInfo>.NotifyListeners(new());
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            EventHolder<UpgradeIncreaseInfo>.AddListener(h_UpgradeIncrease, false);
        }

        private void OnDisable()
        {
            EventHolder<UpgradeIncreaseInfo>.RemoveListener(h_UpgradeIncrease);
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Init();
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            EventHolder<UpgradeChangeInfo>.NotifyListeners(new());
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            var loadData = SaveManager.Instance.Load<UpgradeData>();
            _upgradeLevels[UpgradeType.PlayerHealth] = loadData.PlayerHealthLevel;
            _upgradeLevels[UpgradeType.PlayerSpeed] = loadData.PlayerSpeedLevel;

            var db = DatabaseManager.Instance;
            _upgradeTables[UpgradeType.PlayerHealth] = db.GetTable<PlayerUpgradeData>("PlayerHealth") as PlayerCharacteristicUpgrade;
            _upgradeTables[UpgradeType.PlayerSpeed] = db.GetTable<PlayerUpgradeData>("PlayerSpeed") as PlayerCharacteristicUpgrade;
        }

        private void SaveData()
        {
            var saveData = new UpgradeData();
            saveData.PlayerHealthLevel = _upgradeLevels[UpgradeType.PlayerHealth];
            saveData.PlayerSpeedLevel = _upgradeLevels[UpgradeType.PlayerSpeed];

            SaveManager.Instance.Save(saveData);
        }
        #endregion

        #region METHODS PUBLIC
        public int GetLevel(UpgradeType type)
        {
            return _upgradeLevels[type];
        }

        public int GetMaxLevel<T>(UpgradeType type) where T : ATableData
        {
            var table = _upgradeTables[type] as AbstractTable<T>;
            return table.GetCount();
        }

        public T GetData<T>(UpgradeType type) where T : ATableData
        {
            var table = _upgradeTables[type] as AbstractTable<T>;
            return table.GetDataByIndex((uint)_upgradeLevels[type] - 1);
        }

        public T GetNextData<T>(UpgradeType type) where T : ATableData
        {
            var table = _upgradeTables[type] as AbstractTable<T>;
            return table.GetDataByIndex((uint)_upgradeLevels[type]);
        }
        #endregion
    }

    public enum UpgradeType
    {
        PlayerHealth,
        PlayerSpeed,
    }
}
