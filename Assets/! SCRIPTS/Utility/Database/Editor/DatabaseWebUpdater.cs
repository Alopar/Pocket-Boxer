using Gameplay;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    public static class DatabaseWebUpdater
    {
        #region METHODS PUBLIC
        [MenuItem("Tools/Utility/Download Database")]
        private static void DownloadDatabase()
        {
            Debug.Log("Upload database started:");

            var GUIDs = AssetDatabase.FindAssets("t:DatabaseTable");
            foreach (var guid in GUIDs)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<DatabaseTable>(path);

                if (!asset.IsUpdatable) continue;
                if (asset.URL == string.Empty) continue;

                var task = WebRequestInEditor.Request(asset.URL);
                task.OnRequestSuccess += (string data) => {
                    var datas = data.Replace("\r", string.Empty).Split("\n");
                    asset.UpdateTableData(datas);
                    EditorUtility.SetDirty(asset);
                    AssetDatabase.SaveAssets();
                };
            }
        }
        #endregion
    }
}
