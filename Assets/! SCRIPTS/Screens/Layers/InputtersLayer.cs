using UnityEngine;
using Services.InputSystem;
using Services.ScreenSystem;
using Utility.DependencyInjection;

namespace Screens.Layers
{
    public class InputtersLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private bool _invertDirection;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private IInputService _inputService;

        private const float deathZone = 0.2f;
        private Vector2 _lastDirection = Vector2.zero;
        #endregion

        #region HANDLERS
        private void OnInputChange(InputType type)
        {
            _joystick.OnPointerUp(null);
            if (type.HasFlag(InputType.Joystick))
            {
                ShowScreen();
            }
            else
            {
                HideScreen();
            }
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();
            _inputService.OnInputChange += OnInputChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputService.OnInputChange -= OnInputChange;
        }

        private void Update()
        {
            GetInputOnJoystick();
        }
        #endregion

        #region METHODS PRIVATE
        private void GetInputOnJoystick()
        {
            var pointerDown = false;
            var pointerUp = false;

            if (_lastDirection == Vector2.zero && _joystick.Direction != Vector2.zero)
            {
                pointerDown = true;
            }
            else if (_lastDirection != Vector2.zero && _joystick.Direction == Vector2.zero)
            {
                pointerUp = true;
            }

            var direction = _invertDirection ? _joystick.Direction * -1f : _joystick.Direction;
            var distance = Vector2.Distance(Vector2.zero, _lastDirection);
            var isDeathZone = distance < deathZone;

            _lastDirection = _joystick.Direction;

            if (_joystick.Direction == Vector2.zero && !pointerDown && !pointerUp) return;

            _inputService.SetJoystickData(new(direction, pointerDown, pointerUp, distance, isDeathZone));
        }
        #endregion
    }
}
