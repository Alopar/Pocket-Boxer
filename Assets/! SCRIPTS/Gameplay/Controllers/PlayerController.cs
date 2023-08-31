using System;
using UnityEngine;
using Services.SignalSystem;
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
        [Inject] private IWalletService _wallet;
        [Inject] private ISubscribeService _subscribeService;

        [Find] private WalletComponent _walletComponent;
        [Find] private BatteryComponent _batteryComponent;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void Input(InputInfo info)
        {
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

        [Subscribe]
        private void HidePlayer(HidePlayerInfo info)
        {
            _walletComponent.enabled = false;
            _view.gameObject.SetActive(false);
            _informers.gameObject.SetActive(false);
        }

        [Subscribe]
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
            _subscribeService.Subscribe(this);
        }

        private void OnDisable()
        {
            _subscribeService.Unsubscribe(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            RelaxerInteract(other.gameObject, true);
            SimulatorInteract(other.gameObject, true);
        }

        private void OnTriggerExit(Collider other)
        {
            RelaxerInteract(other.gameObject, false);
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
            _batteryComponent.Init();

            PlaceAgentInStartPosition();
        }

        private void PlaceAgentInStartPosition()
        {
            _navMeshAgent.Warp(transform.position);
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
                if (simulator.EnergyCost > _batteryComponent.Occupied) return;

                if (isEnter)
                {
                    simulator.SetUserBattety(_batteryComponent);
                    SignalSystem<SimulatorChangeFocusInfo>.Send(new(simulator));
                    SignalSystem<ShowScreenInfo>.Send(new(ScreenType.Simulator));
                }
                else
                {
                    simulator.SetUserBattety(null);
                    SignalSystem<CloseScreenInfo>.Send(new(ScreenType.Simulator));
                }
            };
            EntityInteraction(entity, simulator);
        }

        private void RelaxerInteract(GameObject entity, bool isEnter)
        {
            Action<RelaxerController> relaxer = (RelaxerController relaxer) =>
            {
                if (_batteryComponent.IsFull) return;

                if (isEnter)
                {
                    relaxer.SetUserBattety(_batteryComponent);
                    SignalSystem<RelaxerChangeFocusInfo>.Send(new(relaxer));
                    SignalSystem<ShowScreenInfo>.Send(new(ScreenType.Relaxer));
                }
                else
                {
                    relaxer.SetUserBattety(null);
                    SignalSystem<CloseScreenInfo>.Send(new(ScreenType.Relaxer));
                }
            };
            EntityInteraction(entity, relaxer);
        }
        #endregion
    }
}
