using Screens.Layers.Arena;
using System;

namespace Gameplay
{
    [Serializable]
    public struct AbilityCooldownTime
    {
        public AbilityType Type;
        public float Duration;
    }
}
