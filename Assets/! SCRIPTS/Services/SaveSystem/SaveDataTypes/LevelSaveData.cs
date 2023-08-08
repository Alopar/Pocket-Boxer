using System;
using System.Collections.Generic;

namespace Services.SaveSystem
{
    [Serializable]
    public class LevelSaveData : AbstractSaveData
    {
        private const string PREF_NAME = "LEVEL-DATA";

        public int CurrentLevel;
        public int ChunkIndex;
        public List<int> DestroyedBlocks;

        public override string PrefName => PREF_NAME;

        public override T Copy<T>()
        {
            return new LevelSaveData()
            {
                CurrentLevel = CurrentLevel,
                ChunkIndex = ChunkIndex,
                DestroyedBlocks = DestroyedBlocks.GetRange(0, DestroyedBlocks.Count)
            } as T;
        }
    }
}
