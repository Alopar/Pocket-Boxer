using System;

namespace Services.InputSystem
{
    [Flags]
    public enum InputType : byte
    {
        None = 0,
        Joystick = 1,
        Swipe = 2,
        Tap = 4
    }
}
