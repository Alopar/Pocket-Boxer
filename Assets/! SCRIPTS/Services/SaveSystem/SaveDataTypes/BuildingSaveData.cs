using System;
using System.Collections.Generic;

namespace Services.SaveSystem
{
    [Serializable]
    public class BuildingSaveData : AbstractSaveData
    {
        private const string PREF_NAME = "BUILDING-DATA";

        public List<BuildingElem> Buildings;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new BuildingSaveData()
            {
                Buildings = Buildings.GetRange(0, Buildings.Count),
            } as T;
        }
    }

    [Serializable]
    public struct BuildingElem
    {
        public string ID;
        public int Invested;
        public bool Available;
        public bool Constructed;
    }
}
