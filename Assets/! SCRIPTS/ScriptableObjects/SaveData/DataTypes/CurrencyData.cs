using System;
namespace Manager
{
    [Serializable]
    public class CurrencyData : BaseGameData
    {
        private const string PREF_NAME = "CURRENCY-DATA";

        public ulong Money;
        public ulong Diamond;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new CurrencyData()
            {
                Money = Money,
                Diamond = Diamond
            } as T;
        }
    }
}
