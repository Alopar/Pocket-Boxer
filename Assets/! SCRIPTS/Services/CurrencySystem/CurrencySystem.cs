using System;
using System.Collections.Generic;
using Services.SaveSystem;
using Utility.DependencyInjection;

namespace Services.CurrencySystem
{
    public class CurrencySystem : ICurrencyService
    {
        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;

        private Dictionary<CurrencyType, CurrencyDeposite> _deposites = new();
        #endregion

        #region EVENTS
        public event Action<CurrencyType, ulong> OnCurrencyChanged;
        #endregion

        #region CONSTRUCTORS
        public CurrencySystem()
        {
            var currencyData = _saveService.Load<CurrencySaveData>();
            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                var amount = currencyData.CurrencyDatas.Find(e => e.CurrencyType == currencyType).Amount;
                _deposites.Add(currencyType, new CurrencyDeposite(amount));
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void SaveCurrency(CurrencyType type, CurrencyDeposite deposite)
        {
            var saveData = _saveService.Load<CurrencySaveData>();
            var currencyData = saveData.CurrencyDatas.Find(e => e.CurrencyType.Equals(type));
            currencyData.Amount = deposite.Amount;
            _saveService.Save(saveData);
        }
        #endregion

        #region METHODS PUBLIC
        public ulong GetAmount(CurrencyType type)
        {
            return _deposites[type].Amount;
        }

        public void PutCurrency(CurrencyType type, uint value)
        {
            var deposite = _deposites[type];
            deposite.SetCurrency(value);
            SaveCurrency(type, deposite);
            OnCurrencyChanged?.Invoke(type, deposite.Amount);
        }

        public bool TryTakeCurrency(CurrencyType type, uint value)
        {
            var deposite = _deposites[type];
            if (deposite.TryGetCurrency(value))
            {
                SaveCurrency(type, deposite);
                OnCurrencyChanged?.Invoke(type, deposite.Amount);
                return true;
            }

            return false;
        }
        #endregion
    }
}
