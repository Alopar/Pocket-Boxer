using System;

namespace Services.SaveSystem
{
    [Serializable]
    public class CurrencySaveData : AbstractSaveData
    {
        private const string PREF_NAME = "CURRENCY-DATA";

        public ulong Money;
        public ulong Diamond;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new CurrencySaveData()
            {
                Money = Money,
                Diamond = Diamond
            } as T;
        }
    }
}
