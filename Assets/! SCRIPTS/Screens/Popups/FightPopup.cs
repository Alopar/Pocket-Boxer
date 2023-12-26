using UnityEngine;
using Services.SceneLoader;
using Services.ScreenSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class FightPopup : AbstractScreen
    {
        #region FIELDS INSPECTOR
        [SerializeField] private GameObject _activateButton;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISceneLoaderService _sceneLoaderService;
        #endregion

        #region METHODS PUBLIC
        public void ActivateButton()
        {
            _sceneLoaderService.Load("3-ARENA");
        }
        #endregion
    }
}
