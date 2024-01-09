using System;
using UnityEngine;
using Services.InputSystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Services.ScreenSystem;
using Services.CurrencySystem;
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

        [Space(10)]
        [SerializeField] private GameObject _doll;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private IInputService _inputService;
        [Inject] private ISignalService _signalService;
        [Inject] private IScreenService _screenService;
        [Inject] private ICurrencyService _currencyService;

        [Find] private WalletComponent _walletComponent;
        [Find] private BatteryComponent _batteryComponent;
        #endregion

        #region HANDLERS
        [Subscribe(false)]
        private void HidePlayer(HidePlayer info)
        {
            _walletComponent.enabled = false;
            _view.gameObject.SetActive(false);
            _informers.gameObject.SetActive(false);
        }

        [Subscribe(false)]
        private void ShowPlayer(ShowPlayer info)
        {
            _walletComponent.enabled = true;
            _view.gameObject.SetActive(true);
            _informers.gameObject.SetActive(true);
        }

        [Subscribe(true)]
        private void BatteryDischarge(BatteryDischarge info)
        {
            _batteryComponent.Discharge();
        }

        private void InputJoystick(JoystickData data)
        {
            var direction = _camera.transform.TransformDirection(data.Direction);
            direction.y = 0;
            direction.Normalize();

            RotatePlayer(direction);

            var speedDelta = Mathf.Clamp(data.Distance, 1f, data.Distance);
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
        #endregion

        #region UNITY CALLBACKS
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnEnable()
        {
            _signalService.Subscribe(this);
            _inputService.OnJoystick += InputJoystick;
        }

        private void OnDisable()
        {
            _signalService.Unsubscribe(this);
            _inputService.OnJoystick -= InputJoystick;
        }

        private void OnTriggerEnter(Collider other)
        {
            RingInteract(other.gameObject, true);
            RelaxerInteract(other.gameObject, true);
            SimulatorInteract(other.gameObject, true);
        }

        private void OnTriggerExit(Collider other)
        {
            RingInteract(other.gameObject, false);
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

        private void EntityInteraction<T>(GameObject entity, Action<T> callback)
        {
            if (entity.TryGetComponent<T>(out var component))
            {
                callback(component);
            }
        }

        private void ConstractionYardInteract(GameObject entity)
        {
            Action<IInvestable> callback = (IInvestable investable) => {
                uint investition = _investitonByOnce;
                if (_currencyService.TryTakeCurrency(CurrencyType.Money, investition))
                {
                    investable.Invest(investition);
                }
            };
            EntityInteraction(entity, callback);
        }

        private void SimulatorInteract(GameObject entity, bool isEnter)
        {
            Action<SimulatorController> callback = (SimulatorController simulator) =>
            {
                if (simulator.EnergyCost > _batteryComponent.Occupied) return;

                if (isEnter)
                {
                    var doll = Instantiate(_doll);
                    simulator.SetDoll(doll);

                    simulator.SetUserBattety(_batteryComponent);
                    _screenService.ShowScreen(ScreenType.Simulator, simulator);
                }
                else
                {
                    simulator.RemoveDoll();
                    simulator.SetUserBattety(null);
                    _screenService.CloseScreen(ScreenType.Simulator);
                }
            };
            EntityInteraction(entity, callback);
        }

        private void RelaxerInteract(GameObject entity, bool isEnter)
        {
            Action<RelaxerController> callback = (RelaxerController relaxer) =>
            {
                if (_batteryComponent.IsFull) return;

                if (isEnter)
                {
                    var doll = Instantiate(_doll);
                    relaxer.SetDoll(doll);

                    relaxer.SetUserBattety(_batteryComponent);
                    _screenService.ShowScreen(ScreenType.Relaxer, relaxer);
                }
                else
                {
                    relaxer.RemoveDoll();
                    relaxer.SetUserBattety(null);
                    _screenService.CloseScreen(ScreenType.Relaxer);
                }
            };
            EntityInteraction(entity, callback);
        }

        private void RingInteract(GameObject entity, bool isEnter)
        {
            Action<RingController> callback = (RingController ring) =>
            {
                if (_batteryComponent.Occupied == 0) return;

                if (isEnter)
                {
                    _screenService.ShowScreen(ScreenType.Fight);
                }
                else
                {
                    _screenService.CloseScreen(ScreenType.Fight);
                }
            };
            EntityInteraction(entity, callback);
        }
        #endregion

        #region METHODS PUBLIC
        public void PlaceAgentInStartPosition()
        {
            _navMeshAgent.Warp(transform.position);
        }
        #endregion
    }
}
