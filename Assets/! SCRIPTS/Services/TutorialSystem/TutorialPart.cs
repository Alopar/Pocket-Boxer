using System;

namespace Services.TutorialSystem
{
    [Serializable]
    public struct TutorialPart
    {
        public TutorialStep Step;
        public GameplayEvent Event;
    }
}