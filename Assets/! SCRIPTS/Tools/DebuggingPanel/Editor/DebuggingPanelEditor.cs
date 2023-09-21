using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

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

            var foldout = new Foldout()
            {
                text = "Base inspector",
                viewDataKey = "DebuggingPanel-BaseInspector-Foldout",
            };

            InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
            root.Add(foldout);

            return root;
        }
    }
}
