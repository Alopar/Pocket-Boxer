using Cinemachine;
using EventHolder;
using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class SimulatorController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _id;
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _trainDuration;
        #endregion

        #region FIELDS PRIVATE
        [Find] private RewardComponent _rewardComponent;
        #endregion

        #region HANDLERS
        [EventHolder]
        private void StartTrain(StartTrainInfo info)
        {
            if (info.Simulator != this) return;

            _camera.Priority = 10;
            Invoke(nameof(StopTrain), _trainDuration);
            EventHolder<InputControlInfo>.NotifyListeners(new(false));
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            SubscribeService.SubscribeListener(this);
        }

        private void OnDisable()
        {
            SubscribeService.UnsubscribeListener(this);
        }
        #endregion

        #region METHODS PRIVATE
        private void StopTrain()
        {
            _camera.Priority = 0;
            EventHolder<CloseScreenInfo>.NotifyListeners(new(ScreenType.Simulator));
            EventHolder<EndTrainInfo>.NotifyListeners(new());
            EventHolder<InputControlInfo>.NotifyListeners(new(true));
        }
        #endregion

        #region METHODS PUBLIC
        #endregion
    }
}
