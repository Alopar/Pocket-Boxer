using System;
using UnityEngine;

namespace Services.Database
{
    [Serializable]
    public class StatsUpgradeData : AbstractTableData
    {
        [SerializeField] private uint _level;
        [SerializeField] private uint _cost;

        public uint Level => _level;
        public uint Cost => _cost;
    }
}
