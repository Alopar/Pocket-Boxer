using UnityEngine;
using Utility.DependencyInjection;

namespace Services.TutorialSystem
{
    public abstract class TutorialMarker : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] protected GameObject _view;

        [Space(10)]
        [SerializeField] protected TutorialStep _tutorialStep;
        #endregion

        #region FIELDS PRIVATE
        [Inject] protected ITutorialService _tutorialService;
        #endregion

        #region HANDLERS
        protected abstract void TutorialStepChanged(TutorialStep step);
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            TutorialStepChanged(_tutorialService.CurrentStep);
            _tutorialService.OnStepChanged += TutorialStepChanged;
        }

        private void OnDisable()
        {
            _tutorialService.OnStepChanged -= TutorialStepChanged;
        }
        #endregion
    }
}
