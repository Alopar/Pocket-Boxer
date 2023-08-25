using UnityEngine;
using EventHolder;

namespace Gameplay
{
    public class SwipeDetection : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private const float DEAD_ZONE = 80f;

        private Vector2 _tapPosition;
        private Vector2 _swipeDelta;

        private bool _isMobile;
        private bool _isSwiping;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            GetInput();
            CheckSwipe();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _isMobile = Application.isMobilePlatform;
        }

        private void GetInput()
        {
            if (_isMobile)
            {
                DetectTouchInput();
            }
            else
            {
                DetectMouseInput();
            }
        }

        private void DetectMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isSwiping = true;
                _tapPosition = Input.mousePosition;
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
        }

        private void DetectTouchInput()
        {
            if (Input.touchCount == 0) return;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _isSwiping = true;
                _tapPosition = Input.GetTouch(0).position;
                return;
            }
            
            if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ResetSwipe();
            }
        }

        private void CheckSwipe()
        {
            _swipeDelta = Vector2.zero;

            if (_isSwiping)
            {
                if (!_isMobile && Input.GetMouseButton(0))
                {
                    _swipeDelta = (Vector2)Input.mousePosition - _tapPosition;
                }
                else if (Input.touchCount > 0)
                {
                    _swipeDelta = Input.GetTouch(0).position - _tapPosition;
                }
            }

            if (_swipeDelta.magnitude > DEAD_ZONE)
            {
                Vector2 direction;
                if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
                {
                    direction = _swipeDelta.x > 0 ? Vector2.right : Vector2.left;
                }
                else
                {
                    direction = _swipeDelta.y > 0 ? Vector2.up : Vector2.down;
                }
                EventHolder<InputSwipeInfo>.NotifyListeners(new(direction));

                ResetSwipe();
            }
        }

        private void ResetSwipe()
        {
            _isSwiping = false;
            _tapPosition = Vector2.zero;
            _swipeDelta = Vector2.zero;
        }
        #endregion
    }
}
