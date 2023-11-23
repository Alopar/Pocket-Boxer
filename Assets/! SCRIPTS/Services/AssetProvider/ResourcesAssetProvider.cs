using UnityEngine;
using Services.TutorialSystem;

namespace Services.AssetProvider
{
    public class ResourcesAssetProvider : IAssetService
    {
        public TutorialSequence GetTutorialSequence(string assetPath)
        {
            return Resources.Load<TutorialSequence>(assetPath);
        }

    }
}
