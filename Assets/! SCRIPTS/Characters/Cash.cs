using UnityEngine;
using TMPro;
using Utility.MonoPool;
using DG.Tweening;

namespace Gameplay
{
    public class Cash : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _view;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private TextMeshProUGUI _cashText;

        [Space(10)]
        [SerializeField, Range(0, 99)] private float _speed;
        [SerializeField, Range(0, 99)] private float _lifeTime;
        #endregion

        #region FIELDS PRIVATE
        private bool _inPool;
        private Camera _camera;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_inPool) return;

            transform.rotation = _camera.transform.rotation;
            transform.position = transform.position + (Vector3.up * (_speed * Time.deltaTime));
        }
        #endregion

        #region METHODS PRIVATE
        private void Delete()
        {
            _inPool = true;
            MonoPool.Return(this);
        }
        #endregion

        #region METHODS PUBLIC
        public void Init(int cash)
        {
            _inPool = false;
            _cashText.text = $"+{cash}";
            DOVirtual.Float(1f, 0f, _lifeTime, (v) => { _group.alpha = v; }).SetEase(Ease.InExpo).OnComplete(() => {
                Delete();
            });
        }
        #endregion
    }
}