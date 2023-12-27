using System;

namespace Services.SaveSystem
{
    [Serializable]
    public class FightData : AbstractSaveData
    {
        private const string PREF_NAME = "FIGHT-DATA";
        public override string PrefName => PREF_NAME;

        public ushort EnemyLevel;
    }
}
