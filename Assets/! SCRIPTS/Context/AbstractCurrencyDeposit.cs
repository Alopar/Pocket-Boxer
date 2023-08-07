using UnityEngine;
using Utility.Storage;

namespace Gameplay
{
    public abstract class AbstractCurrencyDeposit
    {
        protected uint _amount;
        protected AbstractStorage<int> _storage;

        public uint Amount => _amount;

        public AbstractCurrencyDeposit(AbstractStorage<int> storage)
        {
            _storage = storage;
        }

        public abstract bool TryGetCurrency(uint value);
        public abstract void SetCurrency(uint value);
    }
}