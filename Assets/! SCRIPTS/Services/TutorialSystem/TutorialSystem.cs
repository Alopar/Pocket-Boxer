using System;
using Services.SaveSystem;
using Services.AssetProvider;
using Utility.GameSettings;
using Utility.DependencyInjection;

namespace Services.TutorialSystem
{

    public class TutorialSystem : ITutorialService
    {
        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;
        [Inject] private IAssetService _assetService;

        private readonly TutorialSequence _sequence;
        
        private TutorialStep _currentStep;
        private TutorialSceneMarker _sceneMarker;
        #endregion

        #region PROPERTIES
        public TutorialStep CurrentStep => _currentStep;
        public TutorialSceneMarker CurrentMarker => _sceneMarker;
        #endregion

        #region EVENTS
        public event Action<TutorialStep> OnStepChanged;
        public event Action<TutorialSceneMarker> OnMarkerChanged;
        #endregion

        #region CONSTRUCTORS
        public TutorialSystem()
        {
            _sequence = _assetService.GetTutorialSequence(GameSettings.TutorialSequencePath);
            LoadData();
        }
        #endregion

        #region METHODS PRIVATE
        private void LoadData()
        {
            var loadData = _saveService.Load<TutorialSaveData>();
            _currentStep = loadData.CurrentStep;
        }

        private void SaveData()
        {
            var saveData = new TutorialSaveData() { CurrentStep = _currentStep};
            _saveService.Save(saveData);
        }

        private void StepActions(TutorialStep step)
        {
            // no actions
        }
        #endregion

        #region METHODS PUBLIC
        public void TriggerEvent(GameplayEvent gameplayEvent)
        {
            if (_currentStep == TutorialStep.EndTutorial) return;

            var buffer = _currentStep;
            var sequence = _sequence.Sequence;
            var tutorialPartIndex = sequence.FindIndex(e => e.Step == _currentStep);
            _currentStep = sequence[tutorialPartIndex].Event == gameplayEvent ? sequence[tutorialPartIndex + 1].Step : _currentStep;

            SaveData();
            StepActions(_currentStep);
            OnStepChanged?.Invoke(_currentStep);
        }

        public void SetCurrentMarker(TutorialSceneMarker marker)
        {
            _sceneMarker = marker;
            OnMarkerChanged?.Invoke(marker);
        }
        #endregion
    }
}
