namespace Gameplay
{
    public interface IWalletService
    {
        void SetCurrency<T>(uint value) where T : AbstractCurrencyDeposit;
        bool TryGetCurrency<T>(uint value) where T : AbstractCurrencyDeposit;
    }
}