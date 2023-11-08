using System;

namespace Services.InputSystem
{
    public class InputSystem : IInputService
    {
        #region FIELDS PRIVATE
        private JoystickData _lastJoystick;
        private SwipeData _lastSwipe;
        private TapData _lastTap;

        private InputType _enableInputs = InputType.None;
        #endregion

        #region PROPERTIES
        public JoystickData LastJoystick => _lastJoystick;
        public SwipeData LastSwipe => _lastSwipe;
        public TapData LastTap => _lastTap;

        public InputType EnableInputs {
            get { 
                return _enableInputs; 
            }
            set { 
                _enableInputs = value; 
                OnInputChange?.Invoke(_enableInputs); 
            }
        }
        #endregion

        #region EVENTS
        public event Action<JoystickData> OnJoystick;
        public event Action<SwipeData> OnSwipe;
        public event Action<TapData> OnTap;
        public event Action<InputType> OnInputChange;
        #endregion

        #region METHODS PUBLIC
        public void SetJoystickData(JoystickData data)
        {
            if(!_enableInputs.HasFlag(InputType.Joystick)) return;

            _lastJoystick = data;
            OnJoystick?.Invoke(_lastJoystick);
        }

        public void SetSwipeData(SwipeData data)
        {
            if (!_enableInputs.HasFlag(InputType.Swipe)) return;

            _lastSwipe = data;
            OnSwipe?.Invoke(_lastSwipe);
        }

        public void SetTapData(TapData data)
        {
            if (!_enableInputs.HasFlag(InputType.Tap)) return;

            _lastTap = data;
            OnTap?.Invoke(_lastTap);
        }
        #endregion
    }
}
