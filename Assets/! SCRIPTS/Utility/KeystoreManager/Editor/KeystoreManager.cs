using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Utility.Keystore
{
    public static class KeystoreManager
    {
        private const string ScriptableObjectName = "KeystorePath";

        [InitializeOnLoadMethod]
        private static void FillKeystorePassword()
        {
            var keystoreDatas = GetKeystoreDatas();
            if (keystoreDatas.Count == 0)
            {
                Debug.LogError("user keystore datas is not set!");
                return;
            }

            UserKeystoreData? keystoreData = null;
            foreach (var data in keystoreDatas)
            {
                if (data.KeystorePath == String.Empty) continue;
                if (!File.Exists(data.KeystorePath)) continue;
                if (data.PasswordPath == String.Empty) continue;
                if (!File.Exists(data.PasswordPath)) continue;

                keystoreData = data;
                break;
            }

            if(keystoreData == null)
            {
                Debug.LogError("no one keystore data not have correct path to keystore files!");
                return;
            }

            PlayerSettings.Android.keystoreName = keystoreData.Value.KeystorePath;
            PlayerSettings.Android.keyaliasName = keystoreData.Value.AliasName;
            using (var reader = new StreamReader(keystoreData.Value.PasswordPath))
            {
                var password = reader.ReadLine();
                PlayerSettings.Android.keystorePass = password;
                PlayerSettings.Android.keyaliasPass = password;
                reader.Close();
            }
        }

        private static List<UserKeystoreData> GetKeystoreDatas()
        {
            var keystorePath = Resources.Load<KeystorePath>(ScriptableObjectName);
            if (!keystorePath)
            {
                keystorePath = CreateKeystorePathData();
            }

            return keystorePath.KeystoreDatas;
        }

        private static KeystorePath CreateKeystorePathData()
        {
            var executeScriptPath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            var directoryPath = Path.GetDirectoryName(executeScriptPath);
            var relativePath = directoryPath.Substring(directoryPath.IndexOf("Assets"));

            if (!AssetDatabase.IsValidFolder(@$"{relativePath}\Resources"))
            {
                AssetDatabase.CreateFolder(relativePath, "Resources");
            }

            var keystorePathData = ScriptableObject.CreateInstance<KeystorePath>();
            AssetDatabase.CreateAsset(keystorePathData, @$"{relativePath}\Resources\{ScriptableObjectName}.asset");

            return keystorePathData;
        }
    }
}
