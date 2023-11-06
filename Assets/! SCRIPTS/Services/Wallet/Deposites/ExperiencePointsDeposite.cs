using Services.SignalSystem;
using Services.SaveSystem;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    public class ExperiencePointsDeposite : AbstractCurrencyDeposit
    {
        #region CONSTRUCTORS
        public ExperiencePointsDeposite(ISaveService saveService, ISignalService signalService) : base(saveService, signalService)
        {
            var saveData = saveService.Load<CurrencySaveData>();
            _amount = (uint)saveData.ExperiencePoints;
            _signalService.Send<ExperiencePointsChange>(new(_amount));
        }
        #endregion

        #region METHODS PRIVATE
        private void SaveData()
        {
            var saveData = _saveService.Load<CurrencySaveData>();
            saveData.ExperiencePoints = _amount;
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
                _signalService.Send<ExperiencePointsChange>(new(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            SaveData();
            _signalService.Send<ExperiencePointsChange>(new(_amount));
        }
        #endregion
    }
}