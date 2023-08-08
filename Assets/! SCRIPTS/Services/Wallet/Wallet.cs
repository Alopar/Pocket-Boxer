using System;
using System.Collections.Generic;
using Services.SaveSystem;

namespace Gameplay
{
    public class Wallet : IWalletService
    {
        #region FIELDS PRIVATE
        private ISaveService _saveService;
        private Dictionary<Type, object> _deposites = new();
        #endregion

        #region CONSTRUCTORS
        public Wallet(ISaveService saveService)
        {
            _saveService = saveService;

            _deposites.Add(typeof(MoneyDeposite), new MoneyDeposite(_saveService));
            _deposites.Add(typeof(DiamondDeposite), new DiamondDeposite(_saveService));
        }
        #endregion

        #region METHODS PUBLIC
        public bool TryGetCurrency<T>(uint value) where T : AbstractCurrencyDeposit
        {
            var deposite = _deposites[typeof(T)] as T;
            return deposite.TryGetCurrency(value);
        }

        public void SetCurrency<T>(uint value) where T : AbstractCurrencyDeposit
        {
            var deposite = _deposites[typeof(T)] as T;
            deposite.SetCurrency(value);
        }
        #endregion
    }
}
