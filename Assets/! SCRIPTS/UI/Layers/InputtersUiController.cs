using UnityEngine;
using EventHolder;
using Manager;

namespace Gameplay
{
    public class InputtersUiController : AScreenUiController
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Joystick _joystick;
        [SerializeField] private bool _invertDirection;
        #endregion

        #region FIELDS PRIVATE
        private const float deathZone = 0.2f;
        private Vector2 _lastDirection = Vector2.zero;
        #endregion

        #region HANDLERS
        private void h_InputControl(InputControlInfo info)
        {
            _joystick.OnPointerUp(null);
            if (info.Enable)
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
        private void Awake()
        {
            EventHolder<InputControlInfo>.AddListener(h_InputControl, true);
        }

        private void Start()
        {
            _canvas.worldCamera = Camera.main;
            _canvas.planeDistance = 5f;
        }

        private void Update()
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

            EventHolder<InputInfo>.NotifyListeners(new InputInfo(direction, pointerDown, pointerUp, distance, isDeathZone));

            EventHolder<GameplayEventInfo>.NotifyListeners(new(GameplayEvent.JoysticInput));
        }
        #endregion
    }
}
