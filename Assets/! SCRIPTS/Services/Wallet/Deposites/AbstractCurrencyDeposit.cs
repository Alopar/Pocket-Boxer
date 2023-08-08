using Services.SaveSystem;

namespace Gameplay
{
    public abstract class AbstractCurrencyDeposit
    {
        protected readonly ISaveService _saveService;
        protected uint _amount;
        
        public AbstractCurrencyDeposit(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public uint Amount => _amount;

        public abstract bool TryGetCurrency(uint value);
        public abstract void SetCurrency(uint value);
    }
}