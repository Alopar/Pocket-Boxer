using UnityEngine;

namespace Services.Database
{
    public interface IUpgradeData
    {
        public float Value { get; }
        public uint Cost { get; }
    }
}