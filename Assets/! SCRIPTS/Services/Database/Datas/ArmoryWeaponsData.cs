using System;

namespace Services.Database
{
    [Serializable]
    public class ArmoryWeaponsData : AbstractTableData
    {
        public int id;
        public string name;
        public uint cost;

        public int ID => id;
        public string Name => name;
        public uint Cost => cost;

        public override T Copy<T>()
        {
            var copy = new ArmoryWeaponsData()
            {
                id = id,
                name = name,
                cost = cost
            };
            return copy as T;
        }
    }
}
