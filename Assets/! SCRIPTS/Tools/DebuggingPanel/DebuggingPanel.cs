using System;
using UnityEngine;
using Services.SignalSystem;
using Services.TutorialSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class DebuggingPanel : MonoBehaviour, IActivatable
    {
        #region FIELDS INSPECTOR
        [HideInInspector] public int currencyAmount;
        [HideInInspector] public CurrencyType currencyType;

        [HideInInspector] public TutorialStep tutorialStep;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private IWalletService _wallet;
        [Inject] private ISubscribeService _signals;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void TutorialStep(TutorialStepInfo info)
        {
            tutorialStep = info.TutorialStep;
        }
        #endregion

        #region UNITY CALLBACKS
        public void OnEnable()
        {
            _signals?.Subscribe(this);
        }

        private void OnDisable()
        {
            _signals?.Unsubscribe(this);
        }
        #endregion

        #region METHODS PUBLIC
        public void AddCurrency(CurrencyType type, uint value)
        {
            switch (type)
            {
                case CurrencyType.Money:
                    _wallet.SetCurrency<MoneyDeposite>(value);
                    break;
                case CurrencyType.Diamond:
                    _wallet.SetCurrency<DiamondDeposite>(value);
                    break;
                case CurrencyType.ExperiencePoints:
                    _wallet.SetCurrency<ExperiencePointsDeposite>(value);
                    break;
                case CurrencyType.StrengthPoints:
                    _wallet.SetCurrency<StrengthPointsDeposite>(value);
                    break;
                case CurrencyType.DexterityPoints:
                    _wallet.SetCurrency<DexterityPointsDeposite>(value);
                    break;
                case CurrencyType.EndurancePoints:
                    _wallet.SetCurrency<EndurancePointsDeposite>(value);
                    break;
            }
        }
        #endregion

        //#region INSPECTOR INFORMATIONS
        //#if UNITY_EDITOR
        //        [CustomEditor(typeof(DebuggingPanel))]
        //        public class DebuggingPanelEditor : Editor
        //        {
        //            private DebuggingPanel _target;

        //            void OnEnable()
        //            {
        //                _target = target as DebuggingPanel;
        //            }

        //            public override void OnInspectorGUI()
        //            {
        //                base.OnInspectorGUI();
        //                if (!Application.isPlaying) return;

        //                GUILayout.Label("TUTORIAL INFO");
        //                GUILayout.Label($"Tutorial step: {_target._tutorialStep}");

        //                GUILayout.Space(10);
        //                GUILayout.Label("STATS INFO");
        //                var sm = DependencyContainer.Get<StatsManager>();
        //                GUILayout.Label($"Strength level: {sm.GetLevel(StatType.Strength)}");
        //                GUILayout.Label($"Dexterity level: {sm.GetLevel(StatType.Dexterity)}");
        //                GUILayout.Label($"Endurance level: {sm.GetLevel(StatType.Endurance)}");

        //                GUILayout.Space(10);
        //                var buttons = new Dictionary<string, Action>
        //                {
        //                    { "ADD MONEY", _target.AddMoney},
        //                    { "ADD DIAMOND", _target.AddDiamond},
        //                    { "ADD EXPERIENCE POINTS", _target.AddExperiencePoints},
        //                    { "ADD STRENGTH POINTS", _target.AddStrengthPoints},
        //                    { "ADD DEXTERITY POINTS", _target.AddDexterityPoints},
        //                    { "ADD ENDURANCE POINTS", _target.AddEndurancePoints},
        //                };

        //                foreach (var button in buttons)
        //                {
        //                    if (GUILayout.Button(button.Key))
        //                    {
        //                        button.Value();
        //                    }
        //                }
        //            }
        //        }
        //#endif
        //        #endregion
    }
}
