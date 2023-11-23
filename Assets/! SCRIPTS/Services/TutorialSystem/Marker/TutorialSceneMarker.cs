using UnityEngine;
using Utility.DependencyInjection;

namespace Services.TutorialSystem
{
    [SelectionBase]
    public class TutorialSceneMarker : TutorialMarker, IDependant
    {
        protected override void TutorialStepChanged(TutorialStep step)
        {
            _view.SetActive(step == _tutorialStep);
        }
    }
}
