using System;
using UnityEngine;

namespace Manager
{
    [Serializable]
    public class UpgradeData : BaseGameData
    {
        private const string PREF_NAME = "UPGRADE-DATA";

        [Min(1)] public int PlayerHealthLevel;
        [Min(1)] public int PlayerSpeedLevel;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new UpgradeData()
            {
                PlayerHealthLevel = PlayerHealthLevel,
                PlayerSpeedLevel = PlayerSpeedLevel
            } as T;
        }
    }
}
