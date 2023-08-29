using System;
using UnityEngine;
using UnityEngine.AI;
using Tools;
using Utility.DependencyInjection;

namespace Gameplay
{
    [SelectionBase]
    public abstract partial class CreatureController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Header("BASE SETTINGS:")]
        [SerializeField] protected Transform _view;
        [SerializeField] protected Transform _informers;
        [SerializeField, Range(0, 50)] protected float _moveSpeed = 2f;

        [Space(10)]
        [SerializeField] protected Animator _animator;
        [SerializeField] protected AnimationEventTransmitter _animationTransmitter;
        #endregion

        #region FIELDS PRIVATE
        [Find] protected Collider _collider;
        [Find] protected NavMeshAgent _navMeshAgent;

        protected Camera _camera;
        protected float _currentMoveSpeed;

        protected CharacterAnimation? _lastCharacterAnimation = null;
        protected CharacterAnimation? _bufferCharacterAnimation = null;
        #endregion

        #region UNITY CALLBACKS
        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Start()
        {
            CameraCache();
        }

        protected virtual void LateUpdate()
        {
            SwitchAnimation();
        }
        #endregion

        #region METHODS PRIVATE
        protected virtual void Init()
        {
            _currentMoveSpeed = _moveSpeed;
            _navMeshAgent.speed = _currentMoveSpeed;
        }

        private void CameraCache()
        {
            _camera = Camera.main;
        }

        private void SwitchAnimation()
        {
            if (_bufferCharacterAnimation is not null && _bufferCharacterAnimation != _lastCharacterAnimation)
            {
                _animator.CrossFadeInFixedTime(_bufferCharacterAnimation.ToString(), 0.2f);
                _lastCharacterAnimation = _bufferCharacterAnimation;
                _bufferCharacterAnimation = null;
            }
        }
        #endregion
    }
}
