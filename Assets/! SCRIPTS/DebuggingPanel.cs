using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EventHolder;
using Services.ServiceLocator;
using Services.TutorialSystem;

namespace Gameplay
{
    public class DebuggingPanel : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private TutorialStep _tutorialStep;
        #endregion

        #region HANDLERS
        [EventHolder]
        private void TutorialStep(TutorialStepInfo info)
        {
            _tutorialStep = info.TutorialStep;
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            SubscribeService.SubscribeListener(this);
        }

        private void OnDisable()
        {
            SubscribeService.UnsubscribeListener(this);
        }
        #endregion

        #region METHODS PRIVATE
#if UNITY_EDITOR
        private void AddMoney()
        {
            ServiceLocator.GetService<IWalletService>().SetCurrency<MoneyDeposite>(100000);
        }

        private void AddDiamond()
        {
            ServiceLocator.GetService<IWalletService>().SetCurrency<DiamondDeposite>(1000);
        }
#endif
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
                GUILayout.Label("UPGRADE INFO");

                GUILayout.Space(10);
                var buttons = new Dictionary<string, Action>
                {
                    { "ADD MONEY", _target.AddMoney},
                    { "ADD DIAMOND", _target.AddDiamond},
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
