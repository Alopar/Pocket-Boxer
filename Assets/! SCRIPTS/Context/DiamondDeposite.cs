using UnityEngine;
using EventHolder;
using Utility.Storage;

namespace Gameplay
{
    public class DiamondDeposite : AbstractCurrencyDeposit
    {
        public DiamondDeposite(AbstractStorage<int> storage) : base(storage)
        {
            _amount = (uint)_storage.Load();
        }

        public override bool TryGetCurrency(uint value)
        {
            if (_amount >= value)
            {
                _amount -= value;
                _storage.Save((int)_amount);
                EventHolder<DiamondChangeInfo>.NotifyListeners(new DiamondChangeInfo(_amount));

                return true;
            }

            return false;
        }

        public override void SetCurrency(uint value)
        {
            _amount += value;
            _storage.Save((int)_amount);
            EventHolder<DiamondChangeInfo>.NotifyListeners(new DiamondChangeInfo(_amount));
        }
    }
}