using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Utility.Keystore
{
    public class KeystorePath : ScriptableObject
    {
        [SerializeField] private List<UserKeystoreData> _userKeystoreDatas = new List<UserKeystoreData>();

        public List<UserKeystoreData> KeystoreDatas => _userKeystoreDatas.ToList();
    }

    [Serializable]
    public struct UserKeystoreData
    {
        public string Name;
        public string AliasName;
        public string KeystorePath;
        public string PasswordPath;
    }
}