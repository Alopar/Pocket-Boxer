using EventHolder;
using Services.SaveSystem;

namespace Services.TutorialSystem
{
    public class TutorialSystem
    {
        #region FIELDS PRIVATE
        private TutorialSequence _sequence;
        private ISaveService _saveService;
        private TutorialStep _currentStep;
        #endregion

        #region CONSTRUCTORS
        public TutorialSystem(TutorialSequence sequence, ISaveService saveService)
        {
            _sequence = sequence;
            _saveService = saveService;
            Init();
        }
        #endregion

        #region HANDLERS
        private void GameplayEvent(GameplayEventInfo info)
        {
            if (_currentStep == TutorialStep.EndTutorial) return;

            var buffer = _currentStep;
            var sequence = _sequence.Sequence;
            var tutorialPartIndex = sequence.FindIndex(e => e.Step == _currentStep);
            _currentStep = sequence[tutorialPartIndex].Event == info.GameplayEvent ? sequence[tutorialPartIndex + 1].Step : _currentStep;

            SaveData();
            StepActions(_currentStep);
            EventHolder<TutorialStepInfo>.NotifyListeners(new(_currentStep));
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            LoadData();
            EventHolder<GameplayEventInfo>.AddListener(GameplayEvent, false);
            EventHolder<TutorialStepInfo>.NotifyListeners(new(_currentStep));
        }

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
    }
}
