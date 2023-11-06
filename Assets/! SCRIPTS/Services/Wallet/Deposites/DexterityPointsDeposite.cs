using Services.SignalSystem;
using Services.SaveSystem;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    public class DexterityPointsDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public DexterityPointsDeposite(ISaveService saveService, ISignalService signalService) : base(saveService, signalService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.DexterityPoints;
            _signalService.Send<DexterityPointsChange>(new(_amount));
        }
        #endregion

        #region METHODS PRIVATE
        private void SaveData()
        {
            var saveData = _saveService.Load<CurrencySaveData>();
            saveData.DexterityPoints = _amount;
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
                _signalService.Send<DexterityPointsChange>(new(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            _signalService.Send<DexterityPointsChange>(new(_amount));
        }
        #endregion
    }
}