using UnityEngine;
using TMPro;
using Utility.MonoPool;
using DG.Tweening;

namespace Gameplay
{
    public class FloatupNumeric : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _view;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private TextMeshProUGUI _text;

        [Space(10)]
        [SerializeField] private string _prefix;

        [Space(10)]
        [SerializeField, Range(0, 99)] private float _speed;
        [SerializeField, Range(0, 99)] private float _lifeTime;
        #endregion

        #region FIELDS PRIVATE
        private bool _inPool;
        private Camera _camera;
        #endregion

        #region UNITY CALLBACKS
        private void LateUpdate()
        {
            if (_inPool) return;

            FloatUp();
            LookAtCamera();
        }
        #endregion

        #region METHODS PRIVATE
        private void FloatUp()
        {
            transform.position = transform.position + (Vector3.up * (_speed * Time.deltaTime));
        }

        private void LookAtCamera()
        {
            transform.rotation = _camera.transform.rotation;
        }

        private void Delete()
        {
            _inPool = true;
            MonoPool.Return(this);
        }

        protected void SetTextColor(Color color)
        {
            _text.color = color;
        }
        #endregion

        #region METHODS PUBLIC
        public void Init(int number)
        {
            _camera = Camera.main;
            _canvas.worldCamera = _camera;

            _inPool = false;
            _text.text = $"{_prefix}{number}";
            DOVirtual.Float(1f, 0f, _lifeTime, (v) => { _group.alpha = v; }).SetEase(Ease.InExpo).OnComplete(() => {
                Delete();
            });
        }
        #endregion
    }
}
