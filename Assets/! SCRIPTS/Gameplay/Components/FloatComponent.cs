using UnityEngine;
using DG.Tweening;

namespace Gameplay
{
    public class FloatComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _view;

        [Space(10)]
        [SerializeField, Range(0f, 5f)] private float _rotateSpeed;
        [SerializeField, Range(0f, 5f)] private float _floatHeight;
        [SerializeField, Range(0f, 5f)] private float _floatSpeed;
        [SerializeField] private Ease _floatEase;
        #endregion

        #region METHODS PUBLIC
        public void TurnOn()
        {
            _view.DORotate(new Vector3(0f, 360f, 0f), 1f / _rotateSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
            _view.DOLocalMove(new Vector3(0f, _floatHeight, 0f), 1 / _floatSpeed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(_floatEase);
        }

        public void TurnOff()
        {
            _view.DOKill();
            _view.localRotation = Quaternion.identity;
            _view.localPosition = Vector3.zero;
            _view.localScale = Vector3.one;
        }
        #endregion
    }
}
