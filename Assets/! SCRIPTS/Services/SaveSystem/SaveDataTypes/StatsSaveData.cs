using System;

namespace Services.SaveSystem
{
    [Serializable]
    public class StatsSaveData : AbstractSaveData
    {
        private const string PREF_NAME = "STATS-DATA";
        public override string PrefName => PREF_NAME;

        public ushort StrengthLevel;
        public ushort DexterityLevel;
        public ushort EnduranceLevel;
    }
}
