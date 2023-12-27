using System;
using UnityEngine;

namespace Services.Database
{
    [Serializable]
    public class EnemyData : AbstractTableData
    {
        [SerializeField] private uint _level;
        [SerializeField] private uint _cost;
        [SerializeField] private uint _strength;
        [SerializeField] private uint _dexterity;
        [SerializeField] private uint _endurance;
        [SerializeField] private string _country;
        [SerializeField] private string _name;

        public uint Level => _level;
        public uint Cost => _cost;
        public uint Strength => _strength;
        public uint Dexterity => _dexterity;
        public uint Endurance => _endurance;
        public string Country => _country;
        public string Name => _name;
    }
}
