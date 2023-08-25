using System;

namespace Gameplay
{
    public interface IEquipment
    {
        public EquipmentType Type { get; }
        public InputType InputType { get; }
        public CurrencyType CurrencyType { get; }
        public uint EnergyCost { get; }

        public event Action<float> OnTimerChange;
        public event Action<float> OnProgressChange;
        public event Action OnExploitationEnd;

        public void AddProgress(float progress);
        public void TurnOn();
        public void TurnOff();
    }
}
