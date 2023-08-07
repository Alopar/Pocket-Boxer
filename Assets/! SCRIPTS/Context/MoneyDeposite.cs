using UnityEngine;
using EventHolder;
using Utility.Storage;

namespace Gameplay
{
    public class MoneyDeposite : AbstractCurrencyDeposit
    {
        public MoneyDeposite(AbstractStorage<int> storage) : base(storage)
        {
            _amount = (uint)_storage.Load();
        }

        public override bool TryGetCurrency(uint value)
        {
            if (_amount >= value)
            {
                _amount -= value;
                _storage.Save((int)_amount);
                EventHolder<MoneyChangeInfo>.NotifyListeners(new MoneyChangeInfo(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            _storage.Save((int)_amount);            
            EventHolder<MoneyChangeInfo>.NotifyListeners(new MoneyChangeInfo(_amount));
        }
    }
}