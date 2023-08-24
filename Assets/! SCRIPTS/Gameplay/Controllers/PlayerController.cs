using System;
using UnityEngine;
using Manager;
using EventHolder;
using Services.Database;
using Utility.DependencyInjection;

namespace Gameplay
{
    [SelectionBase]
    public partial class PlayerController : CreatureController
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField, Range(0, 10)] private float _rotationAtTargetSpeed = 10f;

        [Space(10)]
        [SerializeField, Range(0, 100)] private uint _investitonByOnce = 1;
        #endregion

        #region FIELDS PRIVATE
        [Find] private WalletComponent _walletComponent;

        [Inject] private IWalletService _wallet;

        private float _inputDelay = 0f;
        #endregion

        #region HANDLERS
        [EventHolder]
        private void Input(InputInfo info)
        {
            _inputDelay = 0.1f;
            var direction = _camera.transform.TransformDirection(info.Direction);
            direction.y = 0;
            direction.Normalize();

            RotatePlayer(direction);

            var speedDelta = Mathf.Clamp(info.Distance, 1f, info.Distance);
            var currentSpeed = _currentMoveSpeed * speedDelta;
            if (direction == Vector3.zero)
            {
                MovePlayer(0f);
                _bufferCharacterAnimation = CharacterAnimation.Idle;
            }
            else
            {
                MovePlayer(currentSpeed);
                _bufferCharacterAnimation = CharacterAnimation.Run;
            }
        }

        [EventHolder]
        private void HidePlayer(HidePlayerInfo info)
        {
            _walletComponent.enabled = false;
            _view.gameObject.SetActive(false);
            _informers.gameObject.SetActive(false);
        }

        [EventHolder]
        private void ShowPlayer(ShowPlayerInfo info)
        {
            _walletComponent.enabled = true;
            _view.gameObject.SetActive(true);
            _informers.gameObject.SetActive(true);
        }
        #endregion

        #region UNITY CALLBACKS
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void OnEnable()
        {
            SubscribeService.SubscribeListener(this);
        }

        private void OnDisable()
        {
            SubscribeService.UnsubscribeListener(this);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            UpdateTimers();
        }

        private void OnTriggerEnter(Collider other)
        {
            SimulatorInteract(other.gameObject, true);
        }

        private void OnTriggerExit(Collider other)
        {
            SimulatorInteract(other.gameObject, false);
        }

        private void OnTriggerStay(Collider other)
        {
            ConstractionYardInteract(other.gameObject);
        }
        #endregion

        #region METHODS PRIVATE
        protected override void Init()
        {
            base.Init();
            PlaceAgentInStartPosition();
        }

        private void PlaceAgentInStartPosition()
        {
            _navMeshAgent.Warp(transform.position);
        }

        private void UpdateTimers()
        {
            _inputDelay -= Time.deltaTime;
        }

        private void MovePlayer(float speed)
        {
            _navMeshAgent.velocity = transform.forward.normalized * speed;
        }

        private void RotatePlayer(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationAtTargetSpeed);
        }

        private void EntityInteraction<T>(GameObject entity, Action<T> action)
        {
            if (entity.TryGetComponent<T>(out var component))
            {
                action(component);
            }
        }

        private void ConstractionYardInteract(GameObject entity)
        {
            Action<IInvestable> action = (IInvestable investable) => {
                uint investition = _investitonByOnce;
                if (_wallet.TryGetCurrency<MoneyDeposite>(investition))
                {
                    investable.Invest(investition);
                }
            };
            EntityInteraction(entity, action);
        }

        private void SimulatorInteract(GameObject entity, bool isEnter)
        {
            Action<SimulatorController> simulator = (SimulatorController simulator) =>
            {
                if (isEnter)
                {
                    EventHolder<ShowEquipmentScreenInfo>.NotifyListeners(new(simulator));
                }
                else
                {
                    EventHolder<CloseEquipmentSceenInfo>.NotifyListeners(new());
                }
            };
            EntityInteraction(entity, simulator);
        }
        #endregion
    }
}
