using System;

namespace Services.InputSystem
{
    public interface IInputService
    {
        public InputType EnableInputs { get; set; }
        JoystickData LastJoystick { get; }
        SwipeData LastSwipe { get; }
        TapData LastTap { get; }

        event Action<InputType> OnInputChange;
        event Action<JoystickData> OnJoystick;
        event Action<SwipeData> OnSwipe;
        event Action<TapData> OnTap;

        void SetJoystickData(JoystickData data);
        void SetSwipeData(SwipeData data);
        void SetTapData(TapData data);
    }
}
