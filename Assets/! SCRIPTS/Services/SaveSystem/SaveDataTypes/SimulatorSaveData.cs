using System;
using System.Collections.Generic;

namespace Services.SaveSystem
{
    [Serializable]
    public class SimulatorSaveData : AbstractSaveData
    {
        private const string PREF_NAME = "SIMULATORS-DATA";
        public override string PrefName => PREF_NAME;

        public List<SimulatorElem> Simulators;

        public override AbstractSaveData Copy()
        {
            return new SimulatorSaveData()
            {
                Simulators = Simulators.GetRange(0, Simulators.Count),
            };
        }
    }

    [Serializable]
    public struct SimulatorElem
    {
        public string ID;
        public int Invested;
        public bool Available;
        public bool Constructed;
    }
}
