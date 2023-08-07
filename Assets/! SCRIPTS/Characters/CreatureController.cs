using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Gameplay
{
    [SelectionBase]
    public abstract partial class CreatureController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Header("BASE SETTINGS:")]
        [SerializeField] protected Transform _view;
        [SerializeField, Range(0, 50)] protected float _moveSpeed = 2f;

        [Space(10)]
        [SerializeField] protected Healthbar _healthbar;

        [Space(10)]
        [SerializeField] protected Animator _animator;
        [SerializeField] protected AnimationEventTransmitter _animationTransmitter;
        #endregion

        #region FIELDS PRIVATE
        protected Camera _camera;
        protected Collider _collider;
        protected NavMeshAgent _navMeshAgent;
        protected HealthComponent _healthComponent;

        protected float _currentMoveSpeed;

        protected Scaler _scaler;
        protected Blinker _blinker;

        protected CharacterAnimation? _lastCharacterAnimation = null;
        protected CharacterAnimation? _bufferCharacterAnimation = null;

        protected bool _isKnockbacked = false;
        protected Coroutine _knockbackCoroutine;
        #endregion

        #region PROPERTIES
        public bool IsDied => _healthComponent.IsDied;
        #endregion

        #region HANDLERS
        private void h_Hit()
        {
            _blinker.Blink();
            _scaler.Scale();
            Hit();
        }

        private void h_Die()
        {
            if (_knockbackCoroutine != null)
            {
                StopCoroutine(_knockbackCoroutine);
            }

            Die();
        }
        #endregion

        #region UNITY CALLBACKS
        protected virtual void OnEnable()
        {
            _healthComponent.OnHit += h_Hit;
            _healthComponent.OnDie += h_Die;
        }

        protected virtual void OnDisable()
        {
            _healthComponent.OnHit -= h_Hit;
            _healthComponent.OnDie -= h_Die;
        }

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _healthComponent = GetComponent<HealthComponent>();

            _currentMoveSpeed = _moveSpeed;
            _navMeshAgent.speed = _currentMoveSpeed;

            _scaler = GetComponent<Scaler>();
            _blinker = GetComponent<Blinker>();
        }

        protected virtual void Start()
        {
            _camera = Camera.main;
        }

        protected virtual void LateUpdate()
        {
            if (IsDied)
            {
                _bufferCharacterAnimation = CharacterAnimation.Death;
            }

            if (_bufferCharacterAnimation is not null && _bufferCharacterAnimation != _lastCharacterAnimation)
            {
                _animator.CrossFadeInFixedTime(_bufferCharacterAnimation.ToString(), 0.2f);
                _lastCharacterAnimation = _bufferCharacterAnimation;
                _bufferCharacterAnimation = null;
            }
        }
        #endregion

        #region METHODS PRIVATE
        protected abstract void Die();
        protected abstract void Hit();
        #endregion

        #region METHODS PUBLIC
        public void Knockback(Vector3 force)
        {
            if(_knockbackCoroutine != null)
            {
                StopCoroutine(_knockbackCoroutine);
            }

            _knockbackCoroutine = StartCoroutine(Knockbacking(force, 0.25f));
        }
        #endregion

        #region COROUTINES
        IEnumerator Knockbacking(Vector3 force, float duration)
        {
            _isKnockbacked = true;

            var timer = duration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                _navMeshAgent.Move(force);
                yield return null;
            }

            _isKnockbacked = false;
            _knockbackCoroutine = null;
        }
        #endregion
    }
}