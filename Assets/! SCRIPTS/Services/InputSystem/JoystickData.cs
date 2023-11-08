using UnityEngine;

namespace Services.InputSystem
{
    public readonly struct JoystickData
    {
        public readonly Vector2 Direction;
        public readonly bool PointerDown;
        public readonly bool PointerUp;
        public readonly bool IsDeathZone;
        public readonly float Distance;

        public JoystickData(Vector2 direction, bool pointerDown, bool pointerUp, float distance, bool isDeathZone)
        {
            Direction = direction;
            PointerDown = pointerDown;
            PointerUp = pointerUp;
            IsDeathZone = isDeathZone;
            Distance = distance;
        }

        public override string ToString()
        {
            return $"Down: {PointerDown}, Up: {PointerUp}, Activated: {IsDeathZone}";
        }
    }
}
