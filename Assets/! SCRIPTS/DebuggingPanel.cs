using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gameplay.Managers;
using Services.SignalSystem;
using Services.TutorialSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class DebuggingPanel : MonoBehaviour, IActivatable
    {
        #region FIELDS PRIVATE
        [Inject] private ISubscribeService _signals;

        private TutorialStep _tutorialStep;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void TutorialStep(TutorialStepInfo info)
        {
            _tutorialStep = info.TutorialStep;
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            _signals?.Subscribe(this);
        }

        private void OnDisable()
        {
            _signals?.Unsubscribe(this);
        }
        #endregion

        #region METHODS PRIVATE
#if UNITY_EDITOR
        private void AddMoney()
        {
            DependencyContainer.Get<IWalletService>().SetCurrency<MoneyDeposite>(10000);
        }

        private void AddDiamond()
        {
            DependencyContainer.Get<IWalletService>().SetCurrency<DiamondDeposite>(1000);
        }

        private void AddExperiencePoints()
        {
            DependencyContainer.Get<IWalletService>().SetCurrency<ExperiencePointsDeposite>(1000);
        }

        private void AddStrengthPoints()
        {
            DependencyContainer.Get<IWalletService>().SetCurrency<StrengthPointsDeposite>(100);
        }

        private void AddDexterityPoints()
        {
            DependencyContainer.Get<IWalletService>().SetCurrency<DexterityPointsDeposite>(100);
        }

        private void AddEndurancePoints()
        {
            DependencyContainer.Get<IWalletService>().SetCurrency<EndurancePointsDeposite>(100);
        }
#endif
        #endregion

        #region METHODS PUBLIC
        public void Activate()
        {
            OnEnable();
        }
        #endregion

        #region INSPECTOR INFORMATIONS
#if UNITY_EDITOR
        [CustomEditor(typeof(DebuggingPanel))]
        public class DebuggingPanelEditor : Editor
        {
            private DebuggingPanel _target;

            void OnEnable()
            {
                _target = target as DebuggingPanel;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (!Application.isPlaying) return;

                GUILayout.Label("TUTORIAL INFO");
                GUILayout.Label($"Tutorial step: {_target._tutorialStep}");

                GUILayout.Space(10);
                GUILayout.Label("STATS INFO");
                var sm = DependencyContainer.Get<StatsManager>();
                GUILayout.Label($"Strength level: {sm.GetLevel(StatType.Strength)}");
                GUILayout.Label($"Dexterity level: {sm.GetLevel(StatType.Dexterity)}");
                GUILayout.Label($"Endurance level: {sm.GetLevel(StatType.Endurance)}");

                GUILayout.Space(10);
                var buttons = new Dictionary<string, Action>
                {
                    { "ADD MONEY", _target.AddMoney},
                    { "ADD DIAMOND", _target.AddDiamond},
                    { "ADD EXPERIENCE POINTS", _target.AddExperiencePoints},
                    { "ADD STRENGTH POINTS", _target.AddStrengthPoints},
                    { "ADD DEXTERITY POINTS", _target.AddDexterityPoints},
                    { "ADD ENDURANCE POINTS", _target.AddEndurancePoints},
                };

                foreach (var button in buttons)
                {
                    if (GUILayout.Button(button.Key))
                    {
                        button.Value();
                    }
                }
            }
        }
#endif
        #endregion
    }
}
