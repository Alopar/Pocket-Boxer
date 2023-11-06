using UnityEngine;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Services.ScreenSystem;
using Services.TutorialSystem;

namespace Gameplay
{
    public class TutorialLayer : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private TutorialStep _tutorialStep;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void TutorialStep(TutorialStepChange info)
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
        #endregion
    }
}
