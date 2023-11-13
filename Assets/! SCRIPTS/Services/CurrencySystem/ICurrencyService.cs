using System;

namespace Services.CurrencySystem
{
    public interface ICurrencyService
    {
        event Action<CurrencyType, ulong> OnCurrencyChanged;
        ulong GetAmount(CurrencyType type);
        void PutCurrency(CurrencyType type, uint value);
        bool TryTakeCurrency(CurrencyType type, uint value);
    }
}
