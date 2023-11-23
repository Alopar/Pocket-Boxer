namespace Services.TutorialSystem
{
    public class TutorialScreenMarker : TutorialMarker
    {
        protected override void TutorialStepChanged(TutorialStep step)
        {
            _view.SetActive(step == _tutorialStep);
        }
    }
}
