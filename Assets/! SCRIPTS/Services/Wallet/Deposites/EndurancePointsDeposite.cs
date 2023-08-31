using Services.SignalSystem;
using Services.SaveSystem;

namespace Gameplay
{
    public class EndurancePointsDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public EndurancePointsDeposite(ISaveService saveService) : base(saveService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.EndurancePoints;
            SignalSystem<EndurancePointsChangeInfo>.Send(new(_amount));
        }
        #endregion

        #region METHODS PRIVATE
        private void SaveData()
        {
            var saveData = _saveService.Load<CurrencySaveData>();
            saveData.EndurancePoints = _amount;
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
                SignalSystem<EndurancePointsChangeInfo>.Send(new(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            SignalSystem<EndurancePointsChangeInfo>.Send(new(_amount));
        }
        #endregion
    }
}