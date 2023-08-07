using System;

namespace Manager
{
    [Serializable]
    public class TutorialData : BaseGameData
    {
        private const string PREF_NAME = "TUTORIAL-DATA";

        public TutorialStep CurrentStep;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new TutorialData()
            {
                CurrentStep = CurrentStep
            } as T;
        }
    }
}
