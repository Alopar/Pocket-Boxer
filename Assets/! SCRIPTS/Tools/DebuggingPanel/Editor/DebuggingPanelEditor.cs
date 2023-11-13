using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace Gameplay
{
    [CustomEditor(typeof(DebuggingPanel))]
    public class DebuggingPanelEditor : Editor
    {
        #region FIELDS INSPECTOR
        [SerializeField] private VisualTreeAsset _uxml;
        #endregion

        #region UNITY CALLBACKS
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            _uxml.CloneTree(root);

            if (!Application.isPlaying)
            {
                var playModePanel = root.Q<VisualElement>("play-mode-panel");
                playModePanel.style.display = DisplayStyle.None;

                return root;
            }
            else
            {
                var editorModePanel = root.Q<VisualElement>("editor-mode-panel");
                editorModePanel.style.display = DisplayStyle.None;

                var currencyToolbarButton = root.Q<Button>("currency-toolbar-button");
                currencyToolbarButton.RegisterCallback<ClickEvent>(CurrencyToolbarButton_Click);

                var currencyAddButton = root.Q<Button>("currency-add-button");
                currencyAddButton.RegisterCallback<ClickEvent>(CurrencyAddButton_Click);
            }

            return root;
        }
        #endregion

        #region HANDLERS
        private void CurrencyToolbarButton_Click(ClickEvent evt)
        {

        }

        private void CurrencyAddButton_Click(ClickEvent evt)
        {
            var panel = target as DebuggingPanel;
            panel.AddCurrency(panel.currencyType, (uint)panel.currencyAmount);
        }
        #endregion
    }
}
