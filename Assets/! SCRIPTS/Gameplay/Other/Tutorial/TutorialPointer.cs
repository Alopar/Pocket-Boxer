using UnityEngine;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class TutorialPointer : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _content;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;

        private Transform _tutorialTarget;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void h_TutorialObserving(TutorialObserving info)
        {
            // TODO:
            //if(info == null)
            //{
            //    _content.SetActive(false);
            //    return;
            //}

            _tutorialTarget = info.GameObject.transform;
            _content.SetActive(true);
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            _signalService.Subscribe(this);
        }

        private void OnDisable()
        {
            _signalService.Unsubscribe(this);
        }

        private void LateUpdate()
        {
            RotateAtTarget();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _content.SetActive(false);
        }

        private void RotateAtTarget()
        {
            if (_tutorialTarget == null) return;
            transform.LookAt(_tutorialTarget.transform, Vector3.up);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        #endregion
    }
}