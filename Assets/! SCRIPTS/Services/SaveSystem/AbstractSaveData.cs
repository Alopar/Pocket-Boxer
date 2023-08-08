using UnityEngine;

namespace Services.SaveSystem
{
    public abstract class AbstractSaveData
    {
        public abstract string PrefName { get; }
        public abstract T Copy<T>() where T : AbstractSaveData;
    }
}
