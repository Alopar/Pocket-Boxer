using UnityEngine;
using Tools;
using Services.SignalSystem;
using Utility.DependencyInjection;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    [SelectionBase]
    public class BoxerController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Header("BASE SETTINGS:")]
        [SerializeField] protected Transform _view;
        [SerializeField] protected Transform _informers;

        [Space(10)]
        [SerializeField] protected Animator _animator;
        [SerializeField] protected AnimationEventTransmitter _animationTransmitter;

        [Space(10)]
        [SerializeField] private GameObject _doll;
        #endregion

        #region FIELDS PRIVATE
        [Find] private AbilityComponent _abilityComponent;
        #endregion

        #region PROPERTIES
        public AbilityComponent AbilityComponent => _abilityComponent;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void LateUpdate()
        {

        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {

        }
        #endregion

        #region METHODS PUBLIC
        #endregion
    }
}
