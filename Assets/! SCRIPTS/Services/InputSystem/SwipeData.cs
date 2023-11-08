using UnityEngine;

namespace Services.InputSystem
{
    public readonly struct SwipeData
    {
        public readonly Vector2 Direction;

        public SwipeData(Vector2 direction)
        {
            Direction = direction;
        }
    }
}
