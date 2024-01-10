using UnityEngine;
using Utility.DependencyInjection;

namespace Services.TutorialSystem
{
    [SelectionBase]
    public class TutorialSceneMarker : TutorialMarker, IDependant
    {
        protected override void TutorialStepChanged(TutorialStep step)
        {
            if(step == _tutorialStep)
            {
                _view.SetActive(true);
                _tutorialService.SetCurrentMarker(this);
            }
            else
            {
                _view.SetActive(false);
            }
        }
    }
}
