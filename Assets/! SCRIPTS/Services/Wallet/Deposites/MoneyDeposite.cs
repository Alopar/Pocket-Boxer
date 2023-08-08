using EventHolder;
using Services.SaveSystem;

namespace Gameplay
{
    public class MoneyDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public MoneyDeposite(ISaveService saveService) : base(saveService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.Money;
            EventHolder<MoneyChangeInfo>.NotifyListeners(new MoneyChangeInfo(_amount));
        }
        #endregion

        #region METHODS PRIVATE
        private void SaveData()
        {
            var saveData = _saveService.Load<CurrencySaveData>();
            saveData.Money = _amount;
            _saveService.Save(saveData);
        }
        #endregion

        #region METHODS PUBLIC
        public override bool TryGetCurrency(uint value)
        {
            if (_amount >= value)
            {
                _amount -= value;
                SaveData();
                EventHolder<MoneyChangeInfo>.NotifyListeners(new MoneyChangeInfo(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            EventHolder<MoneyChangeInfo>.NotifyListeners(new MoneyChangeInfo(_amount));
        }
        #endregion
    }
}