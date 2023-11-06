using Services.SaveSystem;
using Services.SignalSystem;

namespace Gameplay
{
    public abstract class AbstractCurrencyDeposit
    {
        protected readonly ISaveService _saveService;
        protected readonly ISignalService _signalService;
        protected uint _amount;
        
        public AbstractCurrencyDeposit(ISaveService saveService, ISignalService signalService)
        {
            _saveService = saveService;
            _signalService = signalService;
        }

        public uint Amount => _amount;

        public abstract bool TryGetCurrency(uint value);
        public abstract void SetCurrency(uint value);
    }
}