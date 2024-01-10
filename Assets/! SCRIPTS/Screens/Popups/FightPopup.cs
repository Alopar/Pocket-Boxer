using UnityEngine;
using Services.SceneLoader;
using Services.ScreenSystem;
using Services.TutorialSystem;
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
        [Inject] private ITutorialService _tutorialService;
        #endregion

        #region METHODS PUBLIC
        public void ActivateButton()
        {
            _sceneLoaderService.Load("3-ARENA");
            _tutorialService.TriggerEvent(GameplayEvent.PushFightButton);
        }
        #endregion
    }
}
