using UnityEngine;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Services.TutorialSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    [SelectionBase]
    public class TutorialMarker : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _view;

        [Space(10)]
        [SerializeField] private TutorialStep _tutorialStep;
        [SerializeField] private bool _inUI;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void h_TutorialStep(TutorialStepChange info)
        {
            if(info.TutorialStep == _tutorialStep)
            {
                _view.SetActive(true);
                if (_inUI) return;

                _signalService.Send<TutorialObserving>(new(gameObject));
                return;
            }

            _view.SetActive(false);
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
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            _view.SetActive(false);
        }
        #endregion
    }
}
