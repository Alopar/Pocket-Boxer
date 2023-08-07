using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Manager;
using EventHolder;
using Utility.BehaviourTree;
using Random = UnityEngine.Random;

namespace Gameplay
{
    [SelectionBase]
    public partial class PlayerController : CreatureController
    {
        #region FIELDS INSPECTOR
        #endregion

        #region FIELDS PRIVATE
        private BehaviourTree _behaviourTree;

        private float _inputDelay = 0f;
        #endregion

        #region PROPERTIES
        #endregion

        #region HANDLERS
        private void h_Input(InputInfo info)
        {
            if (IsDied) return;

            _inputDelay = 0.1f;
            var direction = _camera.transform.TransformDirection(info.Direction);
            direction.y = 0;
            direction.Normalize();

            var speedDelta = Mathf.Clamp(info.Distance, 0.75f, info.Distance);
            var currentSpeed = _currentMoveSpeed * speedDelta;
            MovePlayer(direction, currentSpeed);

            _animator.speed = speedDelta;
            _bufferCharacterAnimation = CharacterAnimation.Run;
        }

        private void h_UpgradeChange(UpgradeChangeInfo info)
        {
            UpdateHealth();
            UpdateSpeed();
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void OnEnable()
        {
            base.OnEnable();

            EventHolder<InputInfo>.AddListener(h_Input, true);
            EventHolder<UpgradeChangeInfo>.AddListener(h_UpgradeChange, true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventHolder<InputInfo>.RemoveListener(h_Input);
            EventHolder<UpgradeChangeInfo>.RemoveListener(h_UpgradeChange);
        }

        protected override void Start()
        {
            base.Start();
            InitBehaviour();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            _inputDelay -= Time.deltaTime;
        }
        #endregion

        #region METHODS PRIVATE
        private void MovePlayer(Vector3 direction, float speed)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.ResetPath();
            _navMeshAgent.velocity = direction * speed;
            _navMeshAgent.avoidancePriority = 15;
        }

        private void UpdateHealth()
        {
            throw new NotImplementedException();
        }

        private void UpdateSpeed()
        {
            throw new NotImplementedException();
        }

        protected override void Die()
        {
            throw new NotImplementedException();
        }

        protected override void Hit()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region METHODS PUBLIC
        public void Init()
        {
            _navMeshAgent.Warp(transform.position);

            UpdateSpeed();
        }
        #endregion

        #region COROUTINES
        #endregion

        #region BEHAVIOUR
        private void InitBehaviour()
        {
            if (_behaviourTree != null)
            {
                _behaviourTree.enabled = true;
                return;
            }

            _behaviourTree = gameObject.AddComponent<BehaviourTree>();

            var root = new Root(_behaviourTree, new Selector(new()
            {
                new ConditionNode(CheckInput)
            }));

            _behaviourTree.SetupTree(root);
        }

        // input branch
        private bool CheckInput(Node node)
        {
            if (_inputDelay > 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}