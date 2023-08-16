using System;
using Services.TutorialSystem;

namespace Services.SaveSystem
{
    [Serializable]
    public class TutorialSaveData : AbstractSaveData
    {
        private const string PREF_NAME = "TUTORIAL-DATA";
        public override string PrefName => PREF_NAME;

        public TutorialStep CurrentStep;
    }
}
