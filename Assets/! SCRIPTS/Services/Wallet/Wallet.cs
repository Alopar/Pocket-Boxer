using System;
using System.Collections.Generic;
using Services.SaveSystem;
using Services.SignalSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class Wallet : IWalletService
    {
        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;
        [Inject] private ISignalService _signalService;

        private Dictionary<Type, object> _deposites = new();
        #endregion

        #region CONSTRUCTORS
        public Wallet()
        {
            _deposites.Add(typeof(MoneyDeposite), new MoneyDeposite(_saveService, _signalService));
            _deposites.Add(typeof(DiamondDeposite), new DiamondDeposite(_saveService, _signalService));
            _deposites.Add(typeof(ExperiencePointsDeposite), new ExperiencePointsDeposite(_saveService, _signalService));
            _deposites.Add(typeof(StrengthPointsDeposite), new StrengthPointsDeposite(_saveService, _signalService));
            _deposites.Add(typeof(DexterityPointsDeposite), new DexterityPointsDeposite(_saveService, _signalService));
            _deposites.Add(typeof(EndurancePointsDeposite), new EndurancePointsDeposite(_saveService, _signalService));
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
