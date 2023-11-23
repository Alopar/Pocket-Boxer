using System;

namespace Services.TutorialSystem
{
    public interface ITutorialService
    {
        TutorialStep CurrentStep { get; }
        TutorialSceneMarker CurrentMarker { get; }
        event Action<TutorialStep> OnStepChanged;
        event Action<TutorialSceneMarker> OnMarkerChanged;
        void TriggerEvent(GameplayEvent gameplayEvent);
        void SetCurrentMarker(TutorialSceneMarker marker);
    }
}
