using Services.TutorialSystem;

namespace Services.AssetProvider
{
    public interface IAssetService
    {
        TutorialSequence GetTutorialSequence(string assetPath);
    }
}
