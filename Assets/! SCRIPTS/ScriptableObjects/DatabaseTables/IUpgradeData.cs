using UnityEngine;

namespace Gameplay
{
    public interface IUpgradeData
    {
        public float Value { get; }
        public uint Cost { get; }
    }
}