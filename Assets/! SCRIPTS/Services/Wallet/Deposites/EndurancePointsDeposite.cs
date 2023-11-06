using Services.SignalSystem;
using Services.SaveSystem;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    public class EndurancePointsDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public EndurancePointsDeposite(ISaveService saveService, ISignalService signalService) : base(saveService, signalService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.EndurancePoints;
            _signalService.Send<EndurancePointsChange>(new(_amount));
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
                _signalService.Send<EndurancePointsChange>(new(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            _signalService.Send<EndurancePointsChange>(new(_amount));
        }
        #endregion
    }
}