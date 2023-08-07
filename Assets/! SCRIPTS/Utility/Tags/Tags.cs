using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace Utility.Tags
{
    public static class Tags
    {
        public static Tag? GetTagByString(string tag)
        {
            foreach (var enumTag in Enum.GetValues(typeof(Tag)))
            {
                if (enumTag.ToString() == tag) return (Tag)enumTag;
            }

            return null;
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void CheckTags()
        {
            var tags = InternalEditorUtility.tags.ToList();

            if (!CompareTagFile(tags, out var debugLog))
            {
                Debug.LogError(debugLog);
            }
        }

        [MenuItem("Tools/Utility/Update Tags #&T")]
        private static void UpdateTagFile()
        {
            var tags = InternalEditorUtility.tags.ToList();

            using (var writer = new StreamWriter(CreateFilePath("Tag", "cs")))
            {
                writer.Write(CreateFileBody(tags));
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static bool CompareTagFile(List<string> unityTags, out string log)
        {
            var verified = true;
            var debugLog = new StringBuilder();
            debugLog.AppendLine("TAG COMPARISON:");

            var enumTags = new List<string>();
            foreach (var type in Enum.GetValues(typeof(Tag)))
            {
                enumTags.Add(type.ToString());
            }

            foreach (var enumTag in enumTags)
            {
                if (unityTags.FirstOrDefault(e => e == enumTag) == null)
                {
                    debugLog.AppendLine($"enum tag with name: {enumTag}, not found in unity tags!");
                    verified = false;
                }
            }

            foreach (var unityTag in unityTags)
            {
                if (enumTags.FirstOrDefault(e => e == unityTag) == null)
                {
                    debugLog.AppendLine($"unity tag with name: {unityTag}, not found in enum tags!");
                    verified = false;
                }
            }

            log = debugLog.ToString();
            return verified;
        }

        private static string CreateFileBody(List<string> tags)
        {
            var header = new StringBuilder();
            header.AppendLine("// THIS CODE MACHINE-GENERATED");
            header.AppendLine("// ANY USER CODE WILL BE DELETED");
            header.AppendLine("");

            var body = new StringBuilder();
            body.AppendLine("public enum Tag");
            body.AppendLine("{");
            tags.ForEach(e => body.AppendLine($"    {e},"));
            body.AppendLine("}");

            return header.AppendJoin(String.Empty, body).ToString();
        }

        private static string CreateFilePath(string name, string extensions)
        {
            var executeScriptPath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            var directoryPath = Path.GetDirectoryName(executeScriptPath);

            return $@"{directoryPath}\{name}.{extensions}";
        }
#endif
    }
}