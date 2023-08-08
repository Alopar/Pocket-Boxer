using System;

namespace Services.Database
{
    [Serializable]
    public class PlayerUpgradeData : AbstractTableData, IUpgradeData
    {
        public float value;
        public uint cost;

        public float Value => value;
        public uint Cost => cost;

        public override T Copy<T>()
        {
            var copy = new PlayerUpgradeData()
            {
                value = value,
                cost = cost
            };
            return copy as T;
        }
    }
}
