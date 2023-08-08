using System;
using UnityEngine;

namespace Services.SaveSystem
{
    [Serializable]
    public class UpgradeSaveData : AbstractSaveData
    {
        private const string PREF_NAME = "UPGRADE-DATA";

        [Min(1)] public int PlayerBackpackLevel;
        [Min(1)] public int PlayerBatteryLevel;
        [Min(1)] public int PlayerSpeedLevel;
        [Min(1)] public int BeamPowerLevel;
        [Min(1)] public int BeamDistanceLevel;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new UpgradeSaveData()
            {
                PlayerBackpackLevel = PlayerBackpackLevel,
                PlayerBatteryLevel = PlayerBatteryLevel,
                PlayerSpeedLevel = PlayerSpeedLevel,
                BeamPowerLevel = BeamPowerLevel,
                BeamDistanceLevel = BeamDistanceLevel,

            } as T;
        }
    }
}
