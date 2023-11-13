using System;
using System.Collections.Generic;
using Services.CurrencySystem;

namespace Services.SaveSystem
{
    [Serializable]
    public class CurrencySaveData : AbstractSaveData
    {
        private const string PREF_NAME = "CURRENCY-DATA";
        public override string PrefName => PREF_NAME;

        public List<CurrencyData> CurrencyDatas;
    }

    [Serializable]
    public class CurrencyData
    {
        public CurrencyType CurrencyType;
        public ulong Amount;
    }
}
