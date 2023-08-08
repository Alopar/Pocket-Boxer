using System;
using Manager;

namespace Services.SaveSystem
{
    [Serializable]
    public class TutorialSaveData : AbstractSaveData
    {
        private const string PREF_NAME = "TUTORIAL-DATA";

        public TutorialStep CurrentStep;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new TutorialSaveData()
            {
                CurrentStep = CurrentStep
            } as T;
        }
    }
}
