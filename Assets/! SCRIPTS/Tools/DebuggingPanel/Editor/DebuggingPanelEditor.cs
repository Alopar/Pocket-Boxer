using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace Gameplay
{
    [CustomEditor(typeof(DebuggingPanel))]
    public class DebuggingPanelEditor : Editor
    {
        public VisualTreeAsset uxml;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            uxml.CloneTree(root);

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
            }

            return root;
        }
    }
}
