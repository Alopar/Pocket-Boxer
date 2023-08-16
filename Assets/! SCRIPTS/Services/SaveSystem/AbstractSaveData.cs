using System;

namespace Services.SaveSystem
{
    public abstract class AbstractSaveData
    {
        public abstract string PrefName { get; }
        public virtual AbstractSaveData Copy()
        {
            return (AbstractSaveData)MemberwiseClone();
        }
    }
}
