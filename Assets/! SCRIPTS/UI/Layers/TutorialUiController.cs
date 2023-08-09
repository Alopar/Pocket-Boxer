using UnityEngine;
using EventHolder;
using Services.ScreenSystem;
using Services.TutorialSystem;

namespace Gameplay
{
    public class TutorialUiController : AbstarctScreenController
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private TutorialStep _tutorialStep;
        #endregion

        #region HANDLERS
        private void h_TutorialStep(TutorialStepInfo info)
        {
            if (info.TutorialStep == _tutorialStep)
            {
                ShowScreen();
            }
            else
            {
                HideScreen();
            }
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            HideScreen();
        }

        private void OnEnable()
        {
            EventHolder<TutorialStepInfo>.AddListener(h_TutorialStep, true);
        }

        private void OnDisable()
        {
            EventHolder<TutorialStepInfo>.RemoveListener(h_TutorialStep);
        }
        #endregion
    }
}
