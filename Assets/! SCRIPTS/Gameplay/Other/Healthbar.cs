using System.Collections;
using UnityEngine;
using TMPro;

namespace Gameplay
{
    public class Healthbar : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Canvas _canvas;

        [Space(10)]
        [SerializeField] private RectTransform _filler;
        [SerializeField] private RectTransform _substrate;
        [SerializeField] private TextMeshProUGUI _healthText;

        [Space(10)]
        [SerializeField] private bool _isAlwaysShown;

        [Header("FALLDOWN SETTINGS:")]
        [SerializeField, Range(0, 3)] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        #endregion

        #region FIELDS PRIVATE
        private Camera _camera;
        private HealthComponent _healthComponent;

        private float _currentDelta;
        private Vector2 _fillerSize;
        private Vector2 _substrateSize;
        #endregion

        #region HANDLERS
        private void HealthChange(int currentHealth, int maxHealth)
        {
            _currentDelta = (float)currentHealth / (float)maxHealth;
            _filler.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _fillerSize.x * _currentDelta);
            _healthText.text = currentHealth.ToString();

            if (_currentDelta <= 0)
            {
                _canvas.gameObject.SetActive(false);
            }

            StopAllCoroutines();
            StartCoroutine(Falldown(_duration));
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            _healthComponent.OnHealthChange += HealthChange;
        }

        private void OnDisable()
        {
            _healthComponent.OnHealthChange -= HealthChange;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.rotation = _camera.transform.rotation;
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            ResolveDependency();

            _fillerSize = _filler.rect.size;
            _substrateSize = _substrate.rect.size;

            if (_isAlwaysShown)
            {
                _canvas.gameObject.SetActive(true);
            }
        }

        private void ResolveDependency()
        {
            _healthComponent = GetComponentInParent<HealthComponent>();
        }
        #endregion

        #region METHODS PUBLIC
        public void TurnOn()
        {
            if (_isAlwaysShown) return;
            _canvas.gameObject.SetActive(true);
        }

        public void TurnOff()
        {
            if (_isAlwaysShown) return;
            _canvas.gameObject.SetActive(false);
        }
        #endregion

        #region COROUTINES
        IEnumerator Falldown(float duration)
        {
            var timer = 0f;
            var substrateDelta = _substrate.rect.size.x / _substrateSize.x;
            while (timer <= duration)
            {
                var time = timer / duration;
                var value = _curve.Evaluate(time);

                var delta = Mathf.Lerp(substrateDelta, _currentDelta, value);
                _substrate.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _substrateSize.x * delta);

                timer += Time.deltaTime;
                yield return null;
            }
            _substrate.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _substrateSize.x * Mathf.Lerp(substrateDelta, _currentDelta, 1f));
        }
        #endregion
    }
}
