using System;
using System.Collections.Generic;

namespace Services.SaveSystem
{
    [Serializable]
    public class ArmorySaveData : AbstractSaveData
    {
        private const string PREF_NAME = "ARMORY-DATA";

        public List<ArmoryElem> Weapons;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new ArmorySaveData()
            {
                Weapons = Weapons.GetRange(0, Weapons.Count),
            } as T;
        }
    }

    [Serializable]
    public struct ArmoryElem
    {
        public int ID;
        public bool Equiped;
        public bool Available;
        public bool Buyed;
    }
}
