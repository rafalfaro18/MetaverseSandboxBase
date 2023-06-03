using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MetaverseSandbox
{
    [Serializable]
    public class EnvironmentReference : AssetReferenceT<Environment>
    {
        public EnvironmentReference(string guid) : base(guid)
        {
        }
    }

    [CreateAssetMenu(fileName = "Environments", menuName = "ScriptableObjects/EnvironmentsScriptableObject", order = 2)]
    public class Environments : ScriptableObject
    {
        public List<EnvironmentReference> environments;
        
    }
}