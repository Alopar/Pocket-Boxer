using System;

namespace Services.SaveSystem
{
    [Serializable]
    public class DataWrapper<T> where T : AbstractSaveData
    {
        public T Data;

        public DataWrapper(T data)
        {
            Data = data;
        }
    }
}
