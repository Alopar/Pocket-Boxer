using EventHolder;
using Services.SaveSystem;

namespace Gameplay
{
    public class DiamondDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public DiamondDeposite(ISaveService saveService) : base(saveService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.Diamond;
            EventHolder<DiamondChangeInfo>.NotifyListeners(new DiamondChangeInfo(_amount));
        }
        #endregion

        #region METHODS PRIVATE
        private void SaveData()
        {
            var saveData = _saveService.Load<CurrencySaveData>();
            saveData.Diamond = _amount;
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
                EventHolder<DiamondChangeInfo>.NotifyListeners(new DiamondChangeInfo(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            EventHolder<DiamondChangeInfo>.NotifyListeners(new DiamondChangeInfo(_amount));
        }
        #endregion
    }
}