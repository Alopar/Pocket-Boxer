namespace Services.CurrencySystem
{
    public class CurrencyDeposite
    {
        #region FIELDS PRIVATE
        private ulong _amount;
        #endregion

        #region PROPERTIES
        public ulong Amount => _amount;
        #endregion

        #region CONSTRUCTORS
        public CurrencyDeposite(ulong amount)
        {
            _amount = amount;
        }
        #endregion

        #region METHODS PUBLIC
        public bool TryGetCurrency(uint value)
        {
            if (_amount < value) return false;

            _amount -= value;
            return true;
        }

        public void SetCurrency(uint value)
        {
            _amount += value;
        }
        #endregion
    }
}
