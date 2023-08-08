using System;
using System.Collections.Generic;
using UnityEngine;
using EventHolder;
using Services.SaveSystem;
using Services.ServiceLocator;

namespace Manager
{
    [DefaultExecutionOrder(-5)]
    public class TutorialManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<TutorialPart> _sequence;
        #endregion

        #region FIELDS PRIVATE
        private static TutorialManager _instance;

        private ISaveService _saveService;

        private TutorialStep _currentStep;
        #endregion

        #region PROPERTIES
        public static TutorialManager Instance => _instance;

        public TutorialStep CurrentStep => _currentStep;
        #endregion

        #region HANDLERS
        [EventHolder(false)]
        private void GameplayEvent(GameplayEventInfo info)
        {
            if (_currentStep == TutorialStep.EndTutorial) return;

            var buffer = _currentStep;
            var tutorialPartIndex = _sequence.FindIndex(e => e.Step == _currentStep);
            _currentStep = _sequence[tutorialPartIndex].Event == info.GameplayEvent ? _sequence[tutorialPartIndex + 1].Step : _currentStep;

            SaveData();
            StepActions(_currentStep);
            EventHolder<TutorialStepInfo>.NotifyListeners(new(_currentStep));
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Init();
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            SubscribeService.SubscribeListener(this);
        }

        private void OnDisable()
        {
            SubscribeService.UnsubscribeListener(this);
        }

        private void Start()
        {
            EventHolder<TutorialStepInfo>.NotifyListeners(new(_currentStep));
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            ResolveDependency();
            LoadData();
        }

        private void ResolveDependency()
        {
            _saveService = ServiceLocator.GetService<ISaveService>();
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

    [Serializable]
    public struct TutorialPart
    {
        public TutorialStep Step;
        public GameplayEvent Event;
    }

    public enum GameplayEvent : byte
    {
        StartGame = 0,
        JoysticInput = 1,
        BlockDestroyed,
        OrePickuped,
        OreSelled,
        ChargerBuyed,
        IslandCleared,
        NextIslandTransited,
        Void = 254,
    }

    public enum TutorialStep : byte
    {
        StartTutorial = 0,
        Movement = 1,
        BlockDestruction,
        PickupOre,
        SellOre,
        BuyCharger,
        CleanIsland,
        GoToNextIsland,
        EndTutorial = 254,
    }
}
