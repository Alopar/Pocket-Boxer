using Services.SignalSystem;
using Services.SaveSystem;
using Utility.DependencyInjection;

namespace Services.TutorialSystem
{
    public class TutorialSystem
    {
        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;
        [Inject] private TutorialSequence _sequence;
        
        private TutorialStep _currentStep;
        #endregion

        #region CONSTRUCTORS
        public TutorialSystem()
        {
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
            SignalSystem<TutorialStepInfo>.Send(new(_currentStep));
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            LoadData();
            SignalSystem<GameplayEventInfo>.AddListener(GameplayEvent, false);
            SignalSystem<TutorialStepInfo>.Send(new(_currentStep));
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
