using Services.SignalSystem;
using Services.SaveSystem;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    public class DiamondDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public DiamondDeposite(ISaveService saveService, ISignalService signalService) : base(saveService, signalService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.Diamond;
            _signalService.Send<DiamondChange>(new(_amount));
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
                _signalService.Send<DiamondChange>(new(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            _signalService.Send<DiamondChange>(new(_amount));
        }
        #endregion
    }
}