using System;

namespace Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        void Load(int index, Action callback = null);
        void Load(string name, Action callback = null);
    }
}
