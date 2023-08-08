using System;

namespace Gameplay
{
    public interface IInvestable
    {
        public event Action<int, int> OnInvest;
        public void Invest(uint value);
    }
}