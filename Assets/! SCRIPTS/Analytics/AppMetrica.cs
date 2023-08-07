using System;
using UnityEngine;

namespace Analytics.AppMetrica
{
    [Serializable]
    public struct LevelStartData
    {
        public int level_number;
        public string level_name;
        public int level_count;
        public string level_diff;
        public bool level_random;
        public string level_type;
    }

    [Serializable]
    public struct LevelFinishData
    {
        public int level_number;
        public string level_name;
        public int level_count;
        public string level_diff;
        public bool level_random;
        public string level_type;
        public string result;
        public int time;
        public int progress;
    }
}