using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Services.SaveSystem;
using Utility.DependencyInjection;

namespace Services.TutorialSystem
{

    public class TutorialSystem : ITutorialService
    {
        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Inject] private ISaveService _saveService;

        private readonly TutorialSequence _sequence;
        
        private TutorialStep _currentStep;
        #endregion

        #region CONSTRUCTORS
        public TutorialSystem(TutorialSequence sequence)
        {
            _sequence = sequence;
        }
        #endregion

        #region HANDLERS
        private void GameplayEvent(GameplayEventChange info)
        {
            if (_currentStep == TutorialStep.EndTutorial) return;

            var buffer = _currentStep;
            var sequence = _sequence.Sequence;
            var tutorialPartIndex = sequence.FindIndex(e => e.Step == _currentStep);
            _currentStep = sequence[tutorialPartIndex].Event == info.GameplayEvent ? sequence[tutorialPartIndex + 1].Step : _currentStep;

            SaveData();
            StepActions(_currentStep);
            _signalService.Send<TutorialStepChange>(new(_currentStep));
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
        public void Init()
        {
            LoadData();
            _signalService.AddListener<GameplayEventChange>(GameplayEvent, false);
            _signalService.Send<TutorialStepChange>(new(_currentStep));
        }
        #endregion
    }
}
