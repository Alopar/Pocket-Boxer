using EventHolder;
using Services.SaveSystem;

namespace Gameplay
{
    public class StrengthPointsDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public StrengthPointsDeposite(ISaveService saveService) : base(saveService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.StrengthPoints;
            EventHolder<StrengthPointsChangeInfo>.NotifyListeners(new(_amount));
        }
        #endregion

        #region METHODS PRIVATE
        private void SaveData()
        {
            var saveData = _saveService.Load<CurrencySaveData>();
            saveData.StrengthPoints = _amount;
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
                EventHolder<StrengthPointsChangeInfo>.NotifyListeners(new(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            EventHolder<StrengthPointsChangeInfo>.NotifyListeners(new(_amount));
        }
        #endregion
    }
}