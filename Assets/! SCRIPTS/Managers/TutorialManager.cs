using UnityEngine;
using EventHolder;
using System.Collections.Generic;
using System;

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

        private TutorialStep _currentStep;
        #endregion

        #region PROPERTIES
        public static TutorialManager Instance => _instance;

        public TutorialStep CurrentStep => _currentStep;
        #endregion

        #region HANDLERS
        private void h_GameplayEvent(GameplayEventInfo info)
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
        private void OnEnable()
        {
            EventHolder<GameplayEventInfo>.AddListener(h_GameplayEvent, false);
        }

        private void OnDisable()
        {
            EventHolder<GameplayEventInfo>.RemoveListener(h_GameplayEvent);
        }

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

        private void Start()
        {
            EventHolder<TutorialStepInfo>.NotifyListeners(new(_currentStep));
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            var loadData = SaveManager.Instance.Load<TutorialData>();
            _currentStep = loadData.CurrentStep;
        }

        private void SaveData()
        {
            var saveData = new TutorialData() { CurrentStep = _currentStep};
            SaveManager.Instance.Save(saveData);
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

    public enum GameplayEvent
    {
        StartGame,
        JoysticInput,
        CitizenDie,
        GettingCurrency,
        OpenUpgradeScreen,
        BuyUpgradeGoods,
        CloseUpgradeScreen,
        GateShown,
        GateDamage,
        GateDestroy,
        LairShown,
        LairDamage,
        LairDestroy,
        Void,
    }

    public enum TutorialStep
    {
        StartTutorial,
        Movement,
        CitizenFight,
        CurrencyReward,
        OpenPlayerUpgradeScreen,
        PlayerUpgradeGoods,
        ClosePlayerUpgradeScreen,
        OpenZombieUpgradeScreen,
        ZombieUpgradeGoods,
        CloseZombieUpgradeScreen,
        FirstGateObserve,
        FirstGateFight,
        FirstGateDestroing,
        FirstLairObserve,
        FirstLairFight,
        FirstLairDestroing,
        SecondGateObserve,
        SecondGateFight,
        SecondGateDestroing,
        SecondLairObserve,
        SecondLairFight,
        SecondLairDestroing,
        ThirdGateObserve,
        ThirdGateFight,
        ThirdGateDestroing,
        ThirdLairObserve,
        ThirdLairFight,
        ThirdLairDestroing,
        FourthGateObserve,
        FourthGateFight,
        EndTutorial
    }
}
