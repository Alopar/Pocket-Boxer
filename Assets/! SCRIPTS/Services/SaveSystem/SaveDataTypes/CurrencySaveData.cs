using System;

namespace Services.SaveSystem
{
    [Serializable]
    public class CurrencySaveData : AbstractSaveData
    {
        private const string PREF_NAME = "CURRENCY-DATA";
        public override string PrefName => PREF_NAME;

        public ulong Money;
        public ulong Diamond;
        public ulong ExperiencePoints;
        public ulong StrengthPoints;
        public ulong DexterityPoints;
        public ulong EndurancePoints;
    }
}
