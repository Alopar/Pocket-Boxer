using UnityEngine;
using UnityEngine.AI;
using Manager;
using Services.SceneLoader;
using Utility.GameSettings;
using Utility.DependencyInjection;
using DG.Tweening;

namespace Gameplay
{
    [DefaultExecutionOrder(-100)]
    public class GameBootstrapper : MonoBehaviour
    {
        #region FIELDS PRIVATE
        [Inject] private ISceneLoaderService _sceneLoaderService;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            SelfResolve();
            InitializeSystems();
            InitializeOtherSystems();
            LoadGameScene();
        }
        #endregion

        #region METHODS PRIVATE
        private void SelfResolve()
        {
            DependencyContainer.Inject(this);
        }

        private void InitializeSystems()
        {
            var prefab = Resources.Load("SYSTEMS");
            var systems = Instantiate(prefab);
            systems.name = "[Systems]";

            DontDestroyOnLoad(systems);
        }

        private void InitializeOtherSystems()
        {
            DOTween.Init();
            NavMesh.avoidancePredictionTime = 0.5f;
            Application.targetFrameRate = GameSettings.Data.ApplicationFrameRate;
        }

        private void LoadGameScene()
        {
            var sceneIndex = GameEntryPoint.StartSceneIndex == 0 ? 1 : GameEntryPoint.StartSceneIndex;
            _sceneLoaderService.Load(sceneIndex);
        }
        #endregion
    }
}
